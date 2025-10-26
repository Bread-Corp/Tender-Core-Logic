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

        [HttpGet("fetch")]
        public async Task<IActionResult> GetTenders([FromQuery] int? page, [FromQuery] int? pageSize)
        {
            //if no params, we return all tenders
            if(page == null || pageSize == null)
            {
                var tenders = await _context.Tenders.Include(t => t.Tags).Include(s => s.SupportingDocs).ToListAsync();
                return Ok(tenders);
            }

            //otherwise we paginate
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page and Page Size values must be valid.");

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
            var tender = _context.Tenders.FirstOrDefault(t => t.TenderID == ID);

            if (tender == null)
                return NotFound();

            switch (tender.Source?.ToLowerInvariant())
            {
                case "eskom":
                    tender = _context.EskomTenders.Include(t => t.Tags).Include(s => s.SupportingDocs).FirstOrDefault(t => t.TenderID == ID);
                    break;

                case "etender":
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
