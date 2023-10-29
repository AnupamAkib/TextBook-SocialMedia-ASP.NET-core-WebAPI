using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using textbook_SocialMedia.Models;

namespace textbook_SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        DBContext _dbContext;
        public UserController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddUser(User user)
        {
            var existingUser = _dbContext.Users.Where(u => u.UserName == user.UserName).FirstOrDefault();
            if(existingUser == null)
            {
                _dbContext.Users.Add(user);
                int rowAffected = _dbContext.SaveChanges();
                if(rowAffected > 0)
                {
                    return Ok(new {status = "success", user });
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
