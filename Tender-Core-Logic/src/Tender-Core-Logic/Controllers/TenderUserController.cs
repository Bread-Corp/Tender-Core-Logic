using Microsoft.AspNetCore.Mvc;
using Tender_Core_Logic.Data;
using Tender_Core_Logic.Models;
using Tender_Core_Logic.UserModels;

namespace Tender_Core_Logic.Controllers
{
    [ApiController]
    [Route("[controller]")] // /TenderUser
    public class TenderUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        //implement blob client for profile images

        public TenderUserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterStandardUser([FromBody] StandardUser tenderUser)
        {
            if (tenderUser == null)
            {
                return BadRequest("Possible Null");
            }

            Console.WriteLine(tenderUser);

            try
            {
                var res = await WriteUser(tenderUser);
                return Ok(res);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to deserialise Json request", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> WriteUser(TenderUser user)
        {
            if (ModelState.IsValid)
            {
                switch (user.Role)
                {
                    case "StandardUser":
                        try
                        {
                            if (user is StandardUser standardUser)
                            {
                                var StandardUser = new StandardUser
                                {
                                    //manually map data to object fields, using the request data where possible.
                                    //I have appended the ID in the model class on initialisation. 
                                    FullName = standardUser.FullName,
                                    Email = standardUser.Email,
                                    PhoneNumber = standardUser.PhoneNumber,
                                    Role = standardUser.Role,
                                    ProfilePicture = "",

                                    Address = standardUser.Address,
                                    Tags = standardUser.Tags,
                                };

                                _context.Add(StandardUser);
                            }
                            else
                            {
                                throw new Exception("Invalid user type!");
                            }
                            
                            await _context.SaveChangesAsync();
                            return Ok();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Failed to create TenderUser", ex);
                        }

                     default:
                        return BadRequest("Failed to write TenderUser");
                }
            }
            else
            {
                throw new Exception("ModelState is invalid for this type of User.");
            }
        }
    }
}
