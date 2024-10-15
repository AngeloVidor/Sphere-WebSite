using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SphereWebsite.Data.Models
{
    public class PostWithUserModel
    {
        public PostsModel Post { get; set; }
        public UserModel User { get; set; }
        public bool UserHasVoted { get; set; }
    }
}
