using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SphereWebsite.Data.ApplicationContext;
using SphereWebsite.Data.Models;

namespace SphereWebsite.Data.Repositories
{
    public class GroupPostsRepository : IGroupPostsRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupPostsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GroupPostsModel>> GetAllPostsAsync()
        {
            return await _context.GroupPosts.ToListAsync();
        }

        public async Task<GroupPostsModel> GetPostByIdAsync(int postId)
        {
            return await _context.GroupPosts.FindAsync(postId);
        }

        public async Task<IEnumerable<GroupPostsModel>> GetPostsByGroupIdAsync(int groupId)
        {
            return await _context.GroupPosts.Where(post => post.GroupId == groupId).ToListAsync();
        }

        public async Task AddPostAsync(GroupPostsModel post)
        {
            await _context.GroupPosts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(GroupPostsModel post)
        {
            _context.GroupPosts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(int postId)
        {
            var post = await GetPostByIdAsync(postId);
            if (post != null)
            {
                _context.GroupPosts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
    }
}
