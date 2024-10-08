using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SphereWebsite.Data.Models;

namespace SphereWebSite.Data.Models.Group
{
    public class UserGroup
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; }
        public int GroupId { get; set; }
        public GroupModel Group { get; set; }
    }
}
