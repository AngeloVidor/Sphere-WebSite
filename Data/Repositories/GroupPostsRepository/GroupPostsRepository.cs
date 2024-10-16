using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SphereWebsite.Data.ApplicationContext;
using SphereWebsite.Data.Models;
using SphereWebSite.Data.Models.GroupVote;
using SphereWebSite.Data.Models.PostsVote;

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
            return await _context.GroupPosts.Include(p => p.Votes).ToListAsync();
        }

        public async Task<GroupPostsModel> GetPostByIdAsync(int postId)
        {
            return await _context
                .GroupPosts.Include(p => p.Votes)
                .FirstOrDefaultAsync(p => p.GroupPostID == postId);
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

        public async Task<bool> AddOrUpdateVoteAsync(int postId, int userId, bool isUpvote)
        {
            var post = await _context
                .GroupPosts.Include(p => p.Votes)
                .FirstOrDefaultAsync(p => p.GroupPostID == postId);

            if (post == null)
            {
                return false;
            }

            var existingVote = post.Votes.FirstOrDefault(v => v.UserId == userId);

            if (existingVote != null)
            {
                if (existingVote.IsUpvote != isUpvote)
                {
                    existingVote.IsUpvote = isUpvote;

                    if (isUpvote)
                    {
                        post.Upvotes++;
                        post.Downvotes--;
                    }
                    else
                    {
                        post.Upvotes--;
                        post.Downvotes++;
                    }
                }
            }
            else
            {
                var newVote = new GroupPostsVoteModel
                {
                    PostId = postId,
                    UserId = userId,
                    IsUpvote = isUpvote
                };
                post.Votes.Add(newVote);

                if (isUpvote)
                    post.Upvotes++;
                else
                    post.Downvotes++;
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
