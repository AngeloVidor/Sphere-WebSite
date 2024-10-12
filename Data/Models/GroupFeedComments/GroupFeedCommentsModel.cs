using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SphereWebsite.Data.Models;

namespace SphereWebSite.Data.Models.GroupFeedComments
{
    public class GroupFeedCommentsModel
    {
        [Key]
        public int CommentID { get; set; }

        [Required(ErrorMessage = "O conteúdo do comentário é obrigatório.")]
        public string Content { get; set; }

        [Required]
        public int UserID { get; set; }

        [BindNever]
        public UserModel User { get; set; }

        [Required]
        public int GroupPostID { get; set; }

        [BindNever]
        [ForeignKey("GroupPostID")]
        public GroupPostsModel GroupPost { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
