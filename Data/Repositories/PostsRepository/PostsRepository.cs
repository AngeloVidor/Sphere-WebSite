using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SphereWebsite.Data.ApplicationContext;
using SphereWebsite.Data.Interfaces.PostsInterface;
using SphereWebsite.Data.Models;

namespace SphereWebsite.Data.Repositories.PostsRepository
{
    public class PostsRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PostsModel> CreatePost(PostsModel post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task DeletePost(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<PostsModel>> GetAllPosts()
        {
            return await _context.Posts.Include(p => p.User).ToListAsync();
        }

        public async Task<PostsModel> GetPostById(int postId)
        {
            return await _context
                .Posts.Include(p => p.User)
                .FirstOrDefaultAsync(p => p.PostID == postId);
        }

        public async Task<PostsModel> UpdatePost(PostsModel post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            return post;
        }
    }
}
