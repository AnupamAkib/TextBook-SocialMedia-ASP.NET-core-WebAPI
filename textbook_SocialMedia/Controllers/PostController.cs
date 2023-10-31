using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using textbook_SocialMedia.Models;
using textbook_SocialMedia.Payload.Request;
using textbook_SocialMedia.Services;

namespace textbook_SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        DBContext _dbContext;
        public PostController(DBContext dBContext)
        {
            _dbContext = dBContext;
        }

        [HttpPost]
        [Route("CreatePost")]
        public IActionResult CreatePost(PostRequest reqPost) 
        {
            var userExist = _dbContext.Users.Where(u => u.UserName == reqPost.UserName).Any();
            if (userExist)
            {
                Post post = new Post();
                post.UserName = reqPost.UserName; //when JWT auth implemented, it will take username from token
                post.Content = reqPost.Content;

                _dbContext.Posts.Add(post);
                int rowAffected = _dbContext.SaveChanges();

                if (rowAffected > 0)
                {
                    return Ok(post);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest(new {status = "failed", msg = "user doesn't exist"});
            }
        }

        [HttpPost]
        [Route("GetPosts")]
        public IActionResult GetPosts(string UserName)
        {
            var posts = _dbContext.Posts.Where(u => u.UserName == UserName);
            if (posts.Any())
            {
                return Ok(new {status = "success", PostFound = posts.Count(), posts});
            }
            else
            {
                return NotFound(new {status="failed", msg = $"no posts found for user {UserName}"});
            }
        }
    }
}
