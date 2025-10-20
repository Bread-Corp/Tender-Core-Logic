using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Tender_Core_Logic.Data;
using Tender_Core_Logic.Models;
using Tender_Core_Logic.UserModels;

namespace Tender_Core_Logic.Controllers
{
    [ApiController]
    [Route("[controller]")] // /Watchlist
    public class WatchlistController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WatchlistController(ApplicationDbContext context)
        {
            _context = context;
        }

        private object WatchlistAnalytics(List<BaseTender> watchlistItems)
        {
            var count = watchlistItems.Count;

            var closed = watchlistItems.Where(w => w.ClosingDate < DateTime.Now).ToList().Count;
            var closingSoon = watchlistItems.Where(w => (w.ClosingDate - DateTime.Now).TotalDays <= 7 && w.ClosingDate > DateTime.Now).ToList().Count;
            var closingLater = count - closingSoon - closed;

            return new
            {
                count,
                closed,
                closingSoon,
                closingLater
            };
        }

        [HttpGet("{userID}")]
        public async Task<IActionResult> GetWatchlist(Guid userID, [FromQuery] int? page, [FromQuery] int? pageSize)
        {
            //if no params, we return all tenders
            if (page == null || pageSize == null)
            {
                var watchlist = await _context.User_Tenders
                .Include(uw => uw.FKTender)
                .Where(uw => uw.FKUserID == userID && uw.IsWatched)
                .Select(uw => uw.FKTender)
                .ToListAsync();

                if (watchlist.IsNullOrEmpty())
                    return BadRequest("No tenders watched!");

                var analytics = WatchlistAnalytics(watchlist);

                return Ok(new
                {
                    watchlist,
                    analytics
                });
            }

            //otherwise we paginate
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page and Page Size values must be valid.");

            int skip = ((int)page - 1) * (int)pageSize;
            var totalCount = await _context.User_Tenders.Where(uw => uw.FKUserID == userID && uw.IsWatched).CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var paginatedTenders = await _context.User_Tenders
                .Include(uw => uw.FKTender)
                .Where(uw => uw.FKUserID == userID && uw.IsWatched)
                .Select(uw => uw.FKTender)
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

        [HttpPost("togglewatch/{userID}/{tenderID}")]
        public async Task<IActionResult> ToggleWatch(Guid userID, Guid tenderID)
        {
            var userTender = await _context.User_Tenders.FirstOrDefaultAsync(uw => uw.FKUserID == userID && uw.FKTenderID == tenderID);

            if (userTender == null)
            {
                userTender = new User_Tender
                {
                    FKUserID = userID,
                    FKTenderID = tenderID,
                    IsWatched = true,
                    UserTenderStatus = "Watching",
                };
                
                _context.User_Tenders.Add(userTender);
            }
            else
            {
                userTender.IsWatched = !userTender.IsWatched; //toggles to whatever the boolean is not
                //keep in mind it might be better to delete the entry if we dont need to maintain history or the state of userTenderStatus
                if (userTender.UserTenderStatus == "Watching" && userTender.IsWatched == false)
                {
                    _context.User_Tenders.Remove(userTender);
                }
            }

            await _context.SaveChangesAsync();
            return Ok(userTender);
        }
    }
}
