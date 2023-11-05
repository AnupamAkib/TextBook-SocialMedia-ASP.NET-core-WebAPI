using System.ComponentModel.DataAnnotations;

namespace textbook_SocialMedia.Models.Post
{
    public class Post
    {
        [Key]
        public int PostID { get; set; }
        public string? UserName { get; set; }
        public string? UserFullName { get; set; }
        public string? Content { get; set; }
        public string Privacy { get; set; } = "Public";
        public string TimeDate { get; set; } = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
        public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
    }
}
