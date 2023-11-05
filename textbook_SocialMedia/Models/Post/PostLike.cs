using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace textbook_SocialMedia.Models.Post
{
    public class PostLike
    {
        [Key]
        public int PostLikeId { get; set; }
        [ForeignKey(nameof(PostID))]
        public int PostID { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
    }
}
