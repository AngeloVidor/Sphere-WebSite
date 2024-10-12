using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SphereWebsite.Data.Models;
using SphereWebSite.Data.Models.GroupFeedComments;

namespace SphereWebSite.Data.Models
{
    public class GroupPostCommentsViewModel
    {
        public GroupPostsModel GroupPost { get; set; }
        public List<GroupFeedCommentsModel> Comments { get; set; }
        public GroupFeedCommentsModel NewComment { get; set; }

        public GroupPostCommentsViewModel()
        {
            NewComment = new GroupFeedCommentsModel(); 
        }
    }
}
