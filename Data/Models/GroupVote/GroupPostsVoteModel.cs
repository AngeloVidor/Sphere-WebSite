using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using SphereWebsite.Data.Models;

namespace SphereWebSite.Data.Models.GroupVote
{
    public class GroupPostsVoteModel
    {
        [Key]
        public int VoteId { get; set; }

        [Required]
        public int PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public GroupPostsModel Post { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserModel User { get; set; }
        public ICollection<GroupPostsVoteModel> Votes { get; set; }

        public bool IsUpvote { get; set; }
    }
}
