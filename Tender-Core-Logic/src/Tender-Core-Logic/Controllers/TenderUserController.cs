using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet("fetch/{adminID}")]
        public async Task<IActionResult> FetchUsers(Guid adminID, [FromQuery] string? role, [FromQuery] int? page, [FromQuery] int? pageSize)
        {
            var admin = await _context.Users.FindAsync(adminID);
            
            if (admin == null || admin.Role != "SuperUser")
            {
                return BadRequest("Invalid User");
            }

            if (string.IsNullOrWhiteSpace(role))
            {
                //return all users
                var allUsers = await _context.Users.ToListAsync();
                var userCount = allUsers.Count;
                var standardUserCount = allUsers.Where(u => u.Role == "StandardUser").ToList().Count;
                var superUserCount = allUsers.Where(u => u.Role == "SuperUser").ToList().Count;
                var registeredRecently = allUsers.Where(u => (DateTime.Now - u.DateAppended).TotalDays < 30).ToList().Count;

                return Ok(new
                {
                    allUsers,
                    userCount,
                    standardUserCount,
                    superUserCount,
                    registeredRecently
                });
            }

            //otherwise we paginate
            if (page <= 0 || pageSize <= 0 || page == null || pageSize == null)
                return BadRequest("Page and Page Size values must be valid.");

            int skip = ((int)page - 1) * (int)pageSize;
            var totalCount = await _context.Users.Where(u => u.Role == role).CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            switch (role)
            {
                case "StandardUser":
                    var paginatedStandardUsers = await _context.StandardUsers
                        .Skip(skip)
                        .Take((int)pageSize)
                        .ToListAsync();

                    var responseStandard = new
                    {
                        Data = paginatedStandardUsers,
                        CurrentPage = page,
                        PageSize = pageSize,
                        TotalCount = totalCount,
                        TotalPages = totalPages
                    };

                    return Ok(responseStandard);

                case "SuperUser":
                    var paginatedSuperUsers = await _context.SuperUsers
                        .Skip(skip)
                        .Take((int)pageSize)
                        .ToListAsync();

                    var responseSuper = new
                    {
                        Data = paginatedSuperUsers,
                        CurrentPage = page,
                        PageSize = pageSize,
                        TotalCount = totalCount,
                        TotalPages = totalPages
                    };

                    return Ok(responseSuper);

                default:
                    //return all users
                    var allUsers = await _context.Users.ToListAsync();
                    return Ok(allUsers);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterStandardUser([FromBody] StandardUser tenderUser)
        {
            if (tenderUser == null)
            {
                return BadRequest("Possible Null");
            }

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

        [HttpPost("superuser/register")]
        public async Task<IActionResult> RegisterSuperUser([FromBody] SuperUser tenderUser)
        {
            if (tenderUser == null)
            {
                return BadRequest("Possible Null");
            }

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
            string userID = "";

            //deduplication check
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser != null)
            {
                //return conflict message for frontend
                return Conflict(new
                {
                    status = "duplicate",
                    message = "A user with this email already exists.",
                    existingUserId = existingUser.UserID
                });
            }

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
                                    //appended the ID and return it in the response.
                                    UserID = Guid.NewGuid(),
                                    FullName = standardUser.FullName,
                                    Email = standardUser.Email,
                                    PhoneNumber = standardUser.PhoneNumber,
                                    DateAppended = DateTime.Now,
                                    Role = standardUser.Role,
                                    ProfilePicture = "",

                                    Address = standardUser.Address,
                                    Tags = standardUser.Tags,
                                };

                                _context.Add(StandardUser);

                                userID = StandardUser.UserID.ToString();
                            }
                            else
                            {
                                throw new Exception("Invalid user type!");
                            }
                            
                            await _context.SaveChangesAsync();
                            return Ok(userID); //return userID as a string - to be appended to cognito.
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Failed to create TenderUser", ex);
                        }

                    case "SuperUser":
                        try
                        {
                            if (user is SuperUser superUser)
                            {
                                var SuperUser = new SuperUser
                                {
                                    //manually map data to object fields, using the request data where possible.
                                    //appended the ID and return it in the response.
                                    UserID = Guid.NewGuid(),
                                    FullName = superUser.FullName,
                                    Email = superUser.Email,
                                    PhoneNumber = superUser.PhoneNumber,
                                    DateAppended = DateTime.Now,
                                    ProfilePicture = "",

                                    Organisation = superUser.Organisation,
                                };

                                _context.Add(SuperUser);

                                userID = SuperUser.UserID.ToString();
                            }
                            else
                            {
                                throw new Exception("Invalid user type!");
                            }
                            
                            await _context.SaveChangesAsync();
                            return Ok(userID); //return userID as a string - to be appended to cognito.
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

        [HttpPost("deleteuser/{userID}")]
        public async Task<IActionResult> DeleteUser(Guid userID)
        {
            try
            {
                //find user and remove
                var user = await _context.Users.FindAsync(userID);
                if (user == null)
                {
                    return NotFound(new
                    {
                        status = "not_found",
                        message = "User not found."
                    });
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    status = "deleted",
                    message = "User deleted successfully.",
                    userId = userID
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = $"Failed to delete user: {ex.Message}"
                });
            }
        }

        //DTO for edit so json can be read from body
        public class EditUserDTO
        {
            public string email { get; set; }
            public string fullName { get; set; }
            public string? phoneNumber { get; set; }
            public string? address { get; set; }
            public string[] tags { get; set; } 
        }

        [HttpPost("edit/{userID}")]
        public async Task<IActionResult> EditUser(Guid userID, [FromBody] EditUserDTO model)
        {
            var user = await _context.StandardUsers.FindAsync(userID);
            if (user == null)
            {
                return NotFound(new
                {
                    status = "not_found",
                    message = "User not found."
                });
            }

            try
            {
                user.Email = model.email;
                user.FullName = model.fullName;

                if (model.phoneNumber != null)
                    user.PhoneNumber = model.phoneNumber;

                if (model.address != null)
                    user.Address = model.address;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    status = "Updated",
                    message = "User updated successfully.",
                    userId = userID
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = $"Failed to edit user: {ex.Message}"
                });
            }
        }
    }
}
