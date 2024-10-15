using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using SphereWebsite.Data.Models;

namespace SphereWebSite.Data.Models.PostsVote
{
    public class PostVoteModel
    {
        [Key]
        public int VoteId { get; set; }
        public int PostId { get; set; }

        [ForeignKey("PostId")]
        public PostsModel Post { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; }
        public bool IsUpvote { get; set; }
    }
}
