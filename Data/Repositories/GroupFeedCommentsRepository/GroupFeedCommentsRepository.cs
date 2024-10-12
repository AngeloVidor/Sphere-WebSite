using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SphereWebSite.Business.Interfaces.GroupFeedCommentsInterface;
using SphereWebsite.Data.ApplicationContext;
using SphereWebSite.Data.Models.GroupFeedComments;

namespace SphereWebSite.Data.Repositories.GroupFeedCommentsRepository
{
    public class GroupFeedCommentsRepository : IGroupFeedCommentsRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupFeedCommentsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GroupFeedCommentsModel> AddCommentAsync(GroupFeedCommentsModel comment)
        {
            await _context.GroupFeedComments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = await _context.GroupFeedComments.FindAsync(commentId);
            if (comment != null)
            {
                _context.GroupFeedComments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<GroupFeedCommentsModel> GetCommentByIdAsync(int commentId)
        {
            return await _context
                .GroupFeedComments.Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CommentID == commentId);
        }

        public async Task<IEnumerable<GroupFeedCommentsModel>> GetCommentsByPostIdAsync(
            int groupPostId
        )
        {
            return await _context
                .GroupFeedComments.Where(c => c.GroupPostID == groupPostId)
                .Include(c => c.User)
                .ToListAsync();
        }

        public async Task UpdateCommentAsync(GroupFeedCommentsModel comment)
        {
            _context.GroupFeedComments.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}
