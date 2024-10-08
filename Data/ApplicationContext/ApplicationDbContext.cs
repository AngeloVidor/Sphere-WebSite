using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SphereWebsite.Data.Models;
using SphereWebSite.Data.Models.Group;

namespace SphereWebsite.Data.ApplicationContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<PostsModel> Posts { get; set; }
        public DbSet<CommentsModel> Comments { get; set; }
        public DbSet<GroupModel> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<GroupPostsModel> GroupPosts { get; set; }
    }
}
