using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        public UserController(IConfiguration configuration, DBContext dbContext)
        {
            _dbContext = dbContext;
            _config = configuration;
            _authTokenServices = new AuthTokenServices(configuration);
        }


        [HttpPost]
        [Route("RegisterUser")]
        public IActionResult AddUser(User user)
        {
            var existingUser = _dbContext.Users.Where(u => u.UserName == user.UserName).FirstOrDefault();
            if(existingUser == null)
            {
                _dbContext.Users.Add(user);
                int rowAffected = _dbContext.SaveChanges();
                if(rowAffected > 0)
                {
                    string token = _authTokenServices.GenerateToken(user);
                    return Ok(new {status = "success", token, user });
                }
                else
                {
                    return BadRequest(new {status = "failed"});
                }
            }
            else
            {
                return BadRequest(new { status = "failed", msg = "user already exist" });
            }
        }
    }
}
