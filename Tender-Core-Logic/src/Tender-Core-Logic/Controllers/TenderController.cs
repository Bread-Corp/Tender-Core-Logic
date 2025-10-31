using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Tender_Core_Logic.Data;
using Tender_Core_Logic.Models;
using Tender_Core_Logic.Models.DTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Tender_Logic.Controllers
{
[ApiController]
[Route("[controller]")] // /tender
    public class TenderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TenderController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        //DTO for filtering so json can be read from body
        public class FilterDTO
        {
            public string? search { get; set; } //sets search term, if not null
            public string sort { get; set; } //sorts by date closest by default
            public string[] tags { get; set; } //initial set of tags to filter by, if null, return all
            public string? dateFilter { get; set; } //closing soon or newly added 
            public string[] tagFilter { get; set; } //second set of data if user chooses to add more.
            public string? statusFilter { get; set; } //open or closed
            public string? alphaSort { get; set; } //A-Z or Z-A (takes less priority than sort)
            public string[]? sources { get; set; } //for source filtering
        }

        [HttpPost("fetchFiltered")]
        public async Task<IActionResult> GetFilteredTenders([FromQuery] int? page, [FromQuery] int? pageSize, [FromBody] FilterDTO filterModel)
        {
            //otherwise we paginate
            if (page <= 0 || pageSize <= 0 || page == null || pageSize == null)
                return BadRequest("Page and Page Size values must be valid.");

            var tendersQuery = _context.Tenders.AsNoTracking().AsQueryable();//creates a queryable obj without saveChanges tracking

            //apply filtering
            //apply search filter
            if (!string.IsNullOrWhiteSpace(filterModel?.search))
            {
                string term = filterModel.search.Trim();
                tendersQuery = tendersQuery.Where(t =>
                    EF.Functions.Like(t.Title, $"%{term}%") ||
                    (t.Description != null && EF.Functions.Like(t.Description, $"%{term}%")) ||
                    (t.Source != null && EF.Functions.Like(t.Source, $"%{term}%"))
                    ); //utilises EF.Functions for more-effecient querying
            }

            //merge tags - concat
            var allTags = (filterModel?.tags ?? Array.Empty<string>())
                    .Concat(filterModel?.tagFilter ?? Array.Empty<string>())
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Select(t => t.Trim())
                    .Distinct()
                    .ToArray();


            //apply tags filter
            //apply overlay tag filter
            if (allTags.Length > 0)
            {
                tendersQuery = tendersQuery.Where(t => t.Tags.Any(tag => allTags.Contains(tag.TagName)));
            }

            //apply source filter
            if (filterModel?.sources != null && filterModel.sources.Any(s => !string.IsNullOrWhiteSpace(s)))
            {
                var sources = filterModel.sources.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                tendersQuery = tendersQuery.Where(t => sources.Contains(t.Source));
            }

            //apply date filters
            if (!string.IsNullOrWhiteSpace(filterModel?.dateFilter))
            {
                var today = DateTime.UtcNow;
                if (filterModel.dateFilter.Equals("Closing Soon", StringComparison.OrdinalIgnoreCase))
                {
                    tendersQuery = tendersQuery.Where(t => t.ClosingDate > today && t.ClosingDate <= today.AddDays(7));
                }
                else if (filterModel.dateFilter.Equals("Newly Added", StringComparison.OrdinalIgnoreCase))
                {
                    tendersQuery = tendersQuery.Where(t => t.PublishedDate >= today.AddDays(-30)).OrderByDescending(t => t.PublishedDate);
                }
            }

            //apply status filter
            if (!string.IsNullOrWhiteSpace(filterModel?.statusFilter))
            {
                var statusNorm = filterModel.statusFilter.Trim();
                tendersQuery = tendersQuery.Where(t => t.Status == statusNorm);
            }

            //apply sorting logic
            if (!string.IsNullOrWhiteSpace(filterModel?.sort) || !string.IsNullOrWhiteSpace(filterModel?.alphaSort))
            {
                switch (filterModel.sort, filterModel.alphaSort)
                {
                    case ("Descending", "A-Z"):
                        tendersQuery = tendersQuery.OrderByDescending(t => t.ClosingDate)
                                                   .ThenBy(t => t.Title);
                        break;
                    case ("Descending", "Z-A"):
                        tendersQuery = tendersQuery.OrderByDescending(t => t.ClosingDate)
                                                   .ThenByDescending(t => t.Title);
                        break;
                    case ("Ascending", "A-Z"):
                        tendersQuery = tendersQuery.OrderBy(t => t.ClosingDate)
                                                   .ThenBy(t => t.Title);
                        break;
                    case ("Ascending", "Z-A"):
                        tendersQuery = tendersQuery.OrderBy(t => t.ClosingDate)
                                                   .ThenByDescending(t => t.Title);
                        break;
                    default:
                        tendersQuery = tendersQuery.OrderByDescending(t => t.ClosingDate);
                        break;
                }
            }
            else
            {
                tendersQuery = tendersQuery.OrderByDescending(t => t.ClosingDate);
            }

            //apply Pagination
            int skip = ((int)page - 1) * (int)pageSize;
            var totalCount = await tendersQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var paginatedTenders = await tendersQuery
                .Skip(skip)
                .Take((int)pageSize)
                .Select(t => new BaseTenderDTO
                {
                    TenderID = t.TenderID,
                    Title = t.Title,
                    Status = t.Status,
                    PublishedDate = t.PublishedDate,
                    ClosingDate = t.ClosingDate,
                    Source = t.Source,
                    Description = t.Description,
                    AISummary = t.AISummary,
                    Tags = t.Tags.Select(tag => tag).ToList(),
                    //SupportingDocs = t.SupportingDocs.Select(d => new SupportingDoc { SupportingDocID = d.SupportingDocID, Name = d.Name, URL = d.URL}).ToList()
                })

                //.Include(t => t.Tags)
                //.Include(s => s.SupportingDocs)
                .ToListAsync();

            var response = new
            {
                Data = paginatedTenders,
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            return Ok(response);
        }

        [HttpGet("fetch")]
        public async Task<IActionResult> GetTenders([FromQuery] int? page, [FromQuery] int? pageSize, [FromBody] FilterDTO filterModel)
        {
            //if no params, we return all tenders
            if (page == null || pageSize == null)
            {
                var tenders = await _context.Tenders.Include(t => t.Tags).Include(s => s.SupportingDocs).ToListAsync();
                return Ok(tenders);
            }

            //otherwise we paginate
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page and Page Size values must be valid.");

            //apply Pagination
            int skip = ((int)page - 1) * (int)pageSize;
            var totalCount = await _context.Tenders.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var paginatedTenders = await _context.Tenders.Include(t => t.Tags).Include(s => s.SupportingDocs)
                .Skip(skip)
                .Take((int)pageSize)
                .ToListAsync();

            var response = new
            {
                Data = paginatedTenders,
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            return Ok(response);
        }

        [HttpGet("fetch/{ID}")]
        public IActionResult GetTenderByID(Guid ID)
        {
            var tender = _context.Tenders.Find(ID);

            if (tender == null)
                return NotFound();

            switch (tender.Source?.ToLowerInvariant())
            {
                case "eskom":
                    tender = _context.EskomTenders.Include(t => t.Tags).Include(s => s.SupportingDocs).FirstOrDefault(t => t.TenderID == ID);
                    break;

                case "etenders":
                    tender = _context.eTenders.Include(t => t.Tags).Include(s => s.SupportingDocs).FirstOrDefault(t => t.TenderID == ID);
                    break;

                case "sanral":
                    tender = _context.SanralTenders.Include(t => t.Tags).Include(s => s.SupportingDocs).FirstOrDefault(t => t.TenderID == ID);
                    break;

                case "transnet":
                    tender = _context.TransnetTenders.Include(t => t.Tags).Include(s => s.SupportingDocs).FirstOrDefault(t => t.TenderID == ID);
                    break;

                case "sars":
                    tender = _context.SarsTenders.Include(t => t.Tags).Include(s => s.SupportingDocs).FirstOrDefault(t => t.TenderID == ID);
                    break;

                default:
                    throw new Exception("No inherited Tender instance found. Specify source of requested Tender.");
            }

            return Ok(tender);
        }

        [HttpPost("deletetender/{tenderID}")]
        public async Task<IActionResult> DeleteTender(Guid tenderID)
        {
            try
            {
                //find tender and remove
                var tender = await _context.Tenders.FindAsync(tenderID);
                if (tender == null)
                {
                    return NotFound(new
                    {
                        status = "not_found",
                        message = "Tender not found."
                    });
                }

                _context.Tenders.Remove(tender);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    status = "deleted",
                    message = "Tender deleted successfully.",
                    userId = tenderID
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = $"Failed to delete tender: {ex.Message}"
                });
            }
        }
    }
}
