using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using textbook_SocialMedia.Models;

namespace textbook_SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private DBContext _dbContext;
        private IConfiguration _config;
        public UserController(IConfiguration configuration, DBContext dbContext)
        {
            _dbContext = dbContext;
            _config = configuration;
        }

        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, user.FirstName+" "+user.LastName),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Iss, _config["jwt:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Aud, _config["jwt:Audience"])
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(10), //expire after 10 minutes
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
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
                    string token = GenerateToken(user);
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
