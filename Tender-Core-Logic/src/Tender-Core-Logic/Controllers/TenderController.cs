using Microsoft.AspNetCore.Mvc;
using Tender_Core_Logic.Data;
using Tender_Core_Logic.Models;
using StackExchange.Redis;       
using System.Text.Json;

namespace Tender_Logic.Controllers
{
[ApiController]
[Route("[controller]")] // /tender
    public class TenderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IDatabase _redisDb; // Redis database instance
        public TenderController(ApplicationDbContext applicationDbContext, IConnectionMultiplexer redis)
        {
            _context = applicationDbContext;
            _redisDb = redis.GetDatabase();
        }

        [HttpGet("fetch")]
        public async Task<IEnumerable<ITender>> GetTenders()
        {
            var tenders = _context.Tenders.ToList();
            return tenders;
        }

        [HttpGet("fetch/{ID}")]
        public async Task<IActionResult> GetTenderByID(Guid ID)
        {
            // Define a unique key for this tender in the cache
            var cacheKey = $"tender:{ID}";

            // Try to get the tender from Redis first
            string cachedTenderJson = await _redisDb.StringGetAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedTenderJson))
            {
                // CACHE HIT! The item was found in the cache.
                Console.WriteLine($"CACHE HIT for tender_id: {ID}"); 

                // Deserialize the JSON string from Redis back to a Tender object and return it.
                var cachedTender = JsonSerializer.Deserialize<ITender>(cachedTenderJson);
                return Ok(cachedTender);
            }

            // CACHE MISS! The item was not in the cache.
            Console.WriteLine($"CACHE MISS for tender_id: {ID}"); 

            var tender = _context.Tenders.FirstOrDefault(t => t.TenderID == ID);

            if (tender == null)
            {
                return NotFound();
            }

            var tenderJson = JsonSerializer.Serialize(tender);

            // Set the value in Redis with an expiration time of 1 hour
            await _redisDb.StringSetAsync(cacheKey, tenderJson, TimeSpan.FromHours(1));

            return Ok(tender);
        }
    }
}
