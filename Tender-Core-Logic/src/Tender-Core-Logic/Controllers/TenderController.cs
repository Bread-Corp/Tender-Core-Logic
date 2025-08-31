using Microsoft.AspNetCore.Mvc;
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
        public async Task<IEnumerable<ITender>> GetTenders()
        {
            var tenders = _context.Tenders.ToList();
            return tenders;
        }

        [HttpGet("fetch/{ID}")]
        public IActionResult GetTenderByID(Guid ID)
        {
            var tender = _context.Tenders.FirstOrDefault(t => t.TenderID == ID);

            if (tender == null)
                return NotFound();

            return Ok(tender);
        }
    }
}
