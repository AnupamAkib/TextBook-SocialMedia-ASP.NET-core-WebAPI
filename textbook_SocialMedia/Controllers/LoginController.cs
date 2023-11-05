using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using textbook_SocialMedia.Models;
using textbook_SocialMedia.Payload.Request;
using textbook_SocialMedia.Services;

namespace textbook_SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _configuration;
        private DBContext _dbContext;
        private AuthTokenServices _authTokenServices;
        public LoginController(IConfiguration config, DBContext dbContext)
        {
            _configuration = config;
            _dbContext = dbContext;
            _authTokenServices = new AuthTokenServices(_configuration);
        }
        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            var user = _dbContext.Users.Where(u => u.UserName == request.UserName.Trim()).FirstOrDefault();
            if (user != null)
            {
                string token = _authTokenServices.GenerateToken(user);
                return Ok(new
                {
                    status = "success",
                    token,
                    user
                });
            }
            else
            {
                return BadRequest(new
                {
                    status = "failed"
                });
            }
        }
    }
}
