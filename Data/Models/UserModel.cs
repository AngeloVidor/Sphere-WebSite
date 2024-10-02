using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SphereWebsite.Data.Models
{
    public class UserModel
    {
        [Key]
        public int ID { get; set; }
        public string NickName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<PostsModel> Posts { get; set; } = new List<PostsModel>();
        public string? ProfileImageUrl { get; set; }
        [NotMapped]
        public IFormFile ProfileImage { get; set; }
    }
}
