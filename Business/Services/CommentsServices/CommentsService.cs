using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SphereWebsite.Business.Interfaces.CommentsInterface;
using SphereWebsite.Data.Interfaces.CommentsInterface;
using SphereWebsite.Data.Models;

namespace SphereWebsite.Business.Services.CommentsServices
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommentsRepository _commentsRepository;

        public CommentsService(ICommentsRepository commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }

        public async Task<CommentsModel> AddComment(CommentsModel comments)
        {
            if (comments == null)
            {
                throw new ArgumentNullException(nameof(comments), "Comment cannot be null.");
            }
            return await _commentsRepository.AddComment(comments);
        }

        public async Task DeleteComment(int commentId)
        {
            if (commentId <= 0)
            {
                throw new ArgumentException("Invalid Comment ID.", nameof(commentId));
            }
            await _commentsRepository.DeleteComment(commentId);
        }

        public async Task<CommentsModel> GetCommentById(int commentId)
        {
            if (commentId <= 0)
            {
                throw new ArgumentException("Invalid Comment ID.", nameof(commentId));
            }
            var comment = await _commentsRepository.GetCommentById(commentId);

            if (comment == null)
            {
                throw new KeyNotFoundException("Comment not found.");
            }
            return comment;
        }

        public async Task<IEnumerable<CommentsModel>> GetCommentByPostId(int postId)
        {
            if (postId <= 0)
            {
                throw new ArgumentException("Invalid Post ID.", nameof(postId));
            }
            return await _commentsRepository.GetCommentsByPostId(postId);
        }

        public async Task UpdateComment(CommentsModel comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment), "Comment cannot be null.");
            }
            await _commentsRepository.UpdateComment(comment);
        }
    }
}
