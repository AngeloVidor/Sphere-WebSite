using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SphereWebSite.Data.Models.Group;
using SphereWebSite.Data.Models.GroupVote;
using SphereWebSite.Data.Models.PostsVote;

namespace SphereWebsite.Data.Models
{
    public class GroupPostsModel
    {
        [Key]
        public int GroupPostID { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "User field is required.")]
        public int UserId { get; set; }

        [ValidateNever]
        public UserModel User { get; set; }

        [Required(ErrorMessage = "Group field is required.")]
        public int GroupId { get; set; }

        [ValidateNever]
        public GroupModel Group { get; set; }

        public ICollection<CommentsModel> Comments { get; set; } = new List<CommentsModel>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? ImageUrl { get; set; }
        public List<string> SelectedTags { get; set; } = new List<string>();

        public string Tags
        {
            get => string.Join(",", SelectedTags);
            set =>
                SelectedTags = string.IsNullOrEmpty(value)
                    ? new List<string>()
                    : value.Split(',').ToList();
        }


        public ICollection<GroupPostsVoteModel> Votes { get; set; }
        [NotMapped]
        public int VoteCount => Upvotes - Downvotes;
        public int Upvotes { get; set; } = 0;
        public int Downvotes { get; set; } = 0;
    }
}
