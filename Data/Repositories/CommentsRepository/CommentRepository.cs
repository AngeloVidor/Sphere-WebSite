using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SphereWebsite.Data.ApplicationContext;
using SphereWebsite.Data.Interfaces.CommentsInterface;
using SphereWebsite.Data.Models;

namespace SphereWebsite.Data.Repositories.CommentsRepository
{
    public class CommentRepository : ICommentsRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CommentsModel> AddComment(CommentsModel comments)
        {
            await _context.Comments.AddAsync(comments);
            await _context.SaveChangesAsync();
            return comments;
        }

        public async Task DeleteComment(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CommentsModel> GetCommentById(int commentId)
        {
            return await _context
                .Comments.Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CommentID == commentId);
        }

        public async Task<IEnumerable<CommentsModel>> GetCommentsByPostId(int postId)
        {
            return await _context
                .Comments.Include(c => c.User) 
                .Where(c => c.PostID == postId) 
                .ToListAsync();
        }

        public async Task UpdateComment(CommentsModel comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}
