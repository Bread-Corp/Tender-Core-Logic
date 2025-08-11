using Microsoft.AspNetCore.Mvc;
using Tender_Core_Logic.Data;
using Tender_Core_Logic.Models;
using Newtonsoft.Json;

namespace Tender_Core_Logic.Controllers
{
    [ApiController]
    [Route("[controller]")] // /crawler
    public class CrawlerController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;

        public CrawlerController(HttpClient httpClient, ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            Console.WriteLine("I am here!!");
            var pass = await _context.Database.CanConnectAsync();
            return Ok(new {dbConnection = pass ? "Success" : "Failed"});
        }

        [HttpPost]
        public async Task<IActionResult> WriteTender(IEnumerable<ITender> tenders) //rather parse through an IEnumerable as it is read-only and allows for covariance
        {
            if (tenders == null) return BadRequest("No tenders supplied.");

            //boolean to track successful iteration
            bool success = true;

            //Need to implement - errors and added
            var errors = new List<string>();
            var added = 4;

            foreach (var tender in tenders)
            {
                //check the source of each tender
                var source = tender.Source;
                Console.WriteLine("Saucy: " + source); //REMOVE DEBUGGER!!!

                switch (source) //Consider implementing a factory class that handles the switch logic
                {
                    case "eTender":
                        try
                        {
                            var eTender = new eTender
                            {
                                //Manually map data to object fields
                                TenderID = Guid.NewGuid(),
                                Title = tender.Title,
                                Status = "Open",
                                PublishedDate = tender.PublishedDate,
                                ClosingDate = tender.ClosingDate,
                                DateAppended = DateTime.Now,
                                Source = tender.Source,
                                Tags = tender.Tags,
                                Description = tender.Description,
                                SupportingDocs = tender.SupportingDocs,
                            };

                            _context.Add(eTender); //is this better or async??
                        }
                        catch (Exception ex)
                        {
                            success = false;
                        }

                        break;

                    case "Eskom":
                        try
                        {
                            Console.WriteLine("Here"); //REMOVE DEBUGGER!!!
                            if (tender is EskomTender eskomTender) //EXPLICITLY TYPE IG??
                            {
                                Console.WriteLine("I am exkom"); //REMOVE DEBUGGER!!!
                                var EskomTender = new EskomTender
                                {
                                    //Manually map data to object fields
                                    TenderID = Guid.NewGuid(),
                                    Title = eskomTender.Title,
                                    Status = "Open",
                                    PublishedDate = eskomTender.PublishedDate,
                                    ClosingDate = eskomTender.ClosingDate,
                                    DateAppended = DateTime.Now,
                                    Source = eskomTender.Source,
                                    Tags = eskomTender.Tags,
                                    Description = eskomTender.Description,
                                    SupportingDocs = eskomTender.SupportingDocs,

                                    TenderNumber = eskomTender.TenderNumber,
                                    Reference = eskomTender.Reference,
                                    Audience = eskomTender.Audience,
                                    OfficeLocation = eskomTender.OfficeLocation,
                                    Email = eskomTender.Email,
                                    Address = eskomTender.Address,
                                    Province = eskomTender.Province,
                                };

                                //There is no error handling to check if required feels are filled in.

                                Console.WriteLine("TN/" + EskomTender.TenderNumber); //REMOVE DEBUGGER!!!
                                await _context.AddAsync(EskomTender); //change to add, its better.
                            }
                            else { 
                                success = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            success = false;
                            throw new Exception(source, ex);
                        }

                        break;

                    default:
                        //Log an error in getting the Source
                        success = false;
                        break;
                }
            }
            //set the success flag to true to indicate successful iteration
            //we cannot do this because it will just override the false value
            //instead we initialise success to be true, setting it to false when there is an exception or if the default case is met (null or invalid)
            //success = true;

            if (success && added > 0)
            {
                await _context.SaveChangesAsync();
                //LOG here
            }
            else
            {
                Console.WriteLine("Huge LLLL"); //REMOVE DEBUGGER!!!
                //Log the L
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("getEskom")]
        public async Task<IActionResult> GetEskom()
        {
            var eskomList = new List<EskomTender>();
            var eskomURL = Environment.GetEnvironmentVariable("Eskom_API");

            var getResponse = await _httpClient.GetAsync(eskomURL);

            if (getResponse.IsSuccessStatusCode && getResponse != null)
            {
                //Deserialise into a list
                string json = await getResponse.Content.ReadAsStringAsync();

                try
                {
                    Console.WriteLine("decereal"); //REMOVE DEBUGGER!!!
                    eskomList = JsonConvert.DeserializeObject<List<EskomTender>>(json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("boooo!" + ex); //REMOVE DEBUGGER!!!
                    //throw exception
                }
            }

            var res = await WriteTender(eskomList);

            return res;
        }


    }
}
