using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tender_Core_Logic.Data;

namespace Tender_Core_Logic.Controllers
{
    [ApiController]
    [Route("[controller]")] // /Notification
    public class NotificationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotificationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userID}")]
        public async Task<IActionResult> GetNotifications(Guid userID)
        {
            if (String.IsNullOrEmpty(userID.ToString()))
                return BadRequest("userID null");

            var oldNotifications = await _context.Notifications.Where(n => n.FKUserID == userID).OrderByDescending(n => n.Created).Skip(10).ToListAsync(); //get list of notifs past recent 10
            //if there are any entries, discard them
            
            if(oldNotifications.Any())
            {
                _context.Notifications.RemoveRange(oldNotifications);
                await _context.SaveChangesAsync();
            }

            var notifications = await _context.Notifications.Where(n => n.FKUserID == userID).OrderByDescending(n => n.Created).ToListAsync(); //get list of notifs

            return Ok(new { notifications });
        }
    }
}
