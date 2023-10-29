using System.ComponentModel.DataAnnotations;

namespace textbook_SocialMedia.Models
{
    public class Post
    {
        [Key]
        public int PostID {  get; set; }
        public string UserName {  get; set; }
        public string Content {  get; set; }

        public string TimeDate { get; set; } = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
    }
}
