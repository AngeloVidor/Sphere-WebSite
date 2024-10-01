using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SphereWebsite.Data.Models
{
    public class CommentsModel
    {
        [Key]
        public int CommentID { get; set; }

        [Required]
        public string Content { get; set; }
        [Required]
        public int UserID { get; set; }
        public UserModel User { get; set; }
        [Required]
        public int PostID { get; set; } 
        [ForeignKey("PostID")]
        public PostsModel Post { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
