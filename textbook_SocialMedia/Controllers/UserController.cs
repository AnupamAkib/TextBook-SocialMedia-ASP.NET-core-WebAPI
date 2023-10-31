using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using textbook_SocialMedia.Models;
using textbook_SocialMedia.Services;

namespace textbook_SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private DBContext _dbContext;
        private IConfiguration _config;
        private AuthTokenServices _authTokenServices;
        private TextInfo textInfo;
        public UserController(IConfiguration configuration, DBContext dbContext)
        {
            _dbContext = dbContext;
            _config = configuration;
            _authTokenServices = new AuthTokenServices(configuration);
            textInfo = new CultureInfo("en-US", false).TextInfo; //for upper casing first char in each word
        }


        [HttpPost]
        [Route("RegisterUser")]
        public IActionResult AddUser(RegisterUserRequest userRequest)
        {
            try
            {
                //also check if password has at least 6 length
                if (userRequest.Password != userRequest.ReTypePassword)
                {
                    return BadRequest(new
                    {
                        status = "password didn't matched"
                    });
                }
                else
                {
                    var existingUser = _dbContext.Users.Where(u => u.UserName == userRequest.UserName.Trim()).FirstOrDefault();
                    if (existingUser == null)
                    {
                        User user = new User();
                        user.UserName = userRequest.UserName.Trim();
                        user.Password = userRequest.Password;
                        user.FirstName = textInfo.ToTitleCase(userRequest.FirstName.Trim().ToLower());
                        user.LastName = textInfo.ToTitleCase(userRequest.LastName.Trim().ToLower());
                        user.FullName = user.FirstName + " " + user.LastName;
                        user.Email = userRequest.Email.Trim();
                        user.Gender = userRequest.Gender;

                        _dbContext.Users.Add(user);
                        int rowAffected = _dbContext.SaveChanges();
                        if (rowAffected > 0)
                        {
                            string token = _authTokenServices.GenerateToken(user);
                            return Ok(new { status = "success", token, user });
                        }
                        else
                        {
                            return BadRequest(new { status = "failed", msg = "no row affected in database"});
                        }
                    }
                    else
                    {
                        return BadRequest(new { status = "failed", msg = "user already exist" });
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    status = "failed",
                    msg = ex.Message,
                });
            }
        }
    }
}
