using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
//using textbook_SocialMedia.Models;
using textbook_SocialMedia.Models.Post;
using textbook_SocialMedia.Payload.Request;
using textbook_SocialMedia.Services;

namespace textbook_SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private DBContext _dbContext;
        public PostController(DBContext dBContext)
        {
            _dbContext = dBContext;
        }

        [Authorize]
        [HttpPost]
        [Route("CreatePost")]
        public IActionResult CreatePost(PostRequest reqPost) 
        {
            var _userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var _userFullName = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;

            var userExist = _dbContext.Users.Where(u => u.UserName == _userName).Any();

            if (userExist)
            {
                Post post = new Post();
                post.UserName = _userName; 
                post.UserFullName = _userFullName;
                post.Content = reqPost.Content;
                post.Privacy = reqPost.Privacy;

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
                return BadRequest(new {status = "failed", msg = $"user {_userName} doesn't exist"});
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetUserPosts")]
        public IActionResult GetUserPosts() //get all post of user who currently logged in
        {
            var _userName = User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

            var posts = _dbContext.Posts
                .Where(u => u.UserName == _userName)
                .Include(p => p.Likes)
                .ToList();
            
            if (posts.Any())
            {
                return Ok(new {status = "success", PostFound = posts.Count(), posts});
            }
            else
            {
                return NotFound(new {status="failed", msg = $"no posts found for user {_userName}"});
            }
        }

        [Authorize]
        [HttpPost]
        [Route("LikeUnlike")]
        public IActionResult LikeUnlike([FromQuery] int postID) //if already liked, it will unlike it & vice versa
        {
            var _userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var _userFullName = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value;

            var _post = _dbContext.Posts.Where(p => p.PostID == postID).FirstOrDefault();
            if (_post == null)
            {
                return NotFound(new
                {
                    status = "failed",
                    msg = "Post not found"
                });
            }
            if(_post.Privacy == "Private" && _post.UserName != _userName) //user can like his own private post
            {
                return Unauthorized(new
                {
                    status = "failed",
                    msg = "You are unauthorized, the post is private"
                });
            }
            else //post exist with postID & user can like/unlike
            {
                var alreadyLikedPost = _dbContext.PostLikes
                    .Where(_postlike => _postlike.PostID == postID && _postlike.UserName == _userName)
                    .FirstOrDefault();

                if (alreadyLikedPost != null)
                {
                    _dbContext.PostLikes.Remove(alreadyLikedPost);
                    _dbContext.SaveChanges();
                    return Ok(new
                    {
                        status = "success",
                        msg = "You unliked the post",
                        unlikeInfo = alreadyLikedPost
                    });
                }
                else
                {
                    PostLike _like = new PostLike
                    {
                        PostID = postID,
                        UserName = _userName,
                        UserFullName = _userFullName
                    };
                    _dbContext.PostLikes.Add(_like);
                    _dbContext.SaveChanges();
                    return Ok(new
                    {
                        status = "success",
                        msg = "You liked the post",
                        likeInfo = _like
                    });
                }
            }
        }
    }
}
