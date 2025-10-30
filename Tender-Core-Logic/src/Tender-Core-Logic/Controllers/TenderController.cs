using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tender_Core_Logic.Data;
using Tender_Core_Logic.Models;

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
            var tendersQuery = _context.Tenders.Include(t => t.Tags).Include(s => s.SupportingDocs).AsQueryable();

            //otherwise we paginate
            if (page <= 0 || pageSize <= 0 || page == null || pageSize == null)
                return BadRequest("Page and Page Size values must be valid.");

            //apply filtering
            //apply search filter
            if (!string.IsNullOrWhiteSpace(filterModel?.search))
            {
                string term = filterModel.search.ToLower();
                tendersQuery = tendersQuery.Where(t =>
                    t.Title.ToLower().Contains(term) ||
                    (t.Description != null && t.Description.ToLower().Contains(term)) ||
                    (t.Source != null && t.Source.ToLower().Contains(term))
                    );
            }

            //apply tags filter
            if (filterModel?.tags != null && filterModel.tags.Any(t => !string.IsNullOrWhiteSpace(t)))
            {
                tendersQuery = tendersQuery.Where(t => t.Tags.Any(tag => filterModel.tags.Contains(tag.TagName)));
            }

            //apply overlay tag filter
            if (filterModel?.tagFilter != null && filterModel.tagFilter.Any(t => !string.IsNullOrWhiteSpace(t)))
            {
                tendersQuery = tendersQuery.Where(t => t.Tags.Any(tag => filterModel.tagFilter.Contains(tag.TagName)));
            }

            //apply source filter
            if (filterModel?.sources != null && filterModel.sources.Any(s => !string.IsNullOrWhiteSpace(s)))
            {
                tendersQuery = tendersQuery.Where(t => filterModel.sources.Contains(t.Source));
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
                    tendersQuery = tendersQuery.OrderByDescending(t => t.PublishedDate);
                }
            }

            //apply status filter
            if (!string.IsNullOrWhiteSpace(filterModel?.statusFilter))
            {
                var statusNorm = filterModel.statusFilter.ToLower();
                tendersQuery = tendersQuery.Where(t => t.Status != null && t.Status.ToLower() == statusNorm);
            }

            //apply sorting logic
            if (!string.IsNullOrWhiteSpace(filterModel?.sort))
            {
                switch (filterModel.sort)
                {
                    case "Descending":
                        tendersQuery = tendersQuery.OrderByDescending(t => t.ClosingDate);
                        break;

                    case "Ascending":
                        tendersQuery = tendersQuery.OrderBy(t => t.ClosingDate);
                        break;

                    default:
                        tendersQuery = tendersQuery.OrderByDescending(t => t.ClosingDate);
                        break;
                }
            }

            //alphabetical sorting takes lower priority than main sort
            if (!string.IsNullOrWhiteSpace(filterModel?.alphaSort))
            {
                var ordered = tendersQuery as IOrderedQueryable<BaseTender>;
                if (ordered != null)
                {
                    if (filterModel.alphaSort.Equals("A-Z", StringComparison.OrdinalIgnoreCase))
                        tendersQuery = ordered.ThenBy(t => t.Title);
                    else if (filterModel.alphaSort.Equals("Z-A", StringComparison.OrdinalIgnoreCase))
                        tendersQuery = ordered.ThenByDescending(t => t.Title);
                }
                else
                {
                    // no order was applied yet—so start it
                    if (filterModel.alphaSort.Equals("A-Z", StringComparison.OrdinalIgnoreCase))
                        tendersQuery = tendersQuery.OrderBy(t => t.Title);
                    else if (filterModel.alphaSort.Equals("Z-A", StringComparison.OrdinalIgnoreCase))
                        tendersQuery = tendersQuery.OrderByDescending(t => t.Title);
                }
            }

            //apply Pagination
            int skip = ((int)page - 1) * (int)pageSize;
            var totalCount = await tendersQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var paginatedTenders = await tendersQuery
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
