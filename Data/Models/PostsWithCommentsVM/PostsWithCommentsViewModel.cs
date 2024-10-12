using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SphereWebsite.Data.Models.PostsWithComments
{
    public class PostsWithCommentsViewModel
    {
        public PostsModel Post { get; set; }
        public List<CommentsModel> Comments { get; set; }
    }
}
