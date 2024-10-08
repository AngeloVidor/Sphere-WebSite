using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SphereWebsite.Data.Models;

namespace SphereWebSite.Data.Models.Group
{
    public class GroupModel
    {
        [Key]
        public int GroupID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public string GroupImageUrl { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public ICollection<UserModel> Users { get; set; } = new List<UserModel>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
        public int UserCount => UserGroups?.Count ?? 0;
        public ICollection<GroupPostsModel> Posts { get; set; } = new List<GroupPostsModel>();
    }
}
