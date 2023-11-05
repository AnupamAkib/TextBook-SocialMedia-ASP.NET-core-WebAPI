using Microsoft.EntityFrameworkCore;
using textbook_SocialMedia.Models;
using textbook_SocialMedia.Models.Post;

namespace textbook_SocialMedia
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        public DbSet<PostLike> PostLikes { get; set; }
    }
}
