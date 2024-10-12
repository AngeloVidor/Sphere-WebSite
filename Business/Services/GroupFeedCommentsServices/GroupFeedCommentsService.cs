using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SphereWebSite.Business.Interfaces.GroupFeedCommentsInterface;
using SphereWebSite.Data.Models.GroupFeedComments;

namespace SphereWebSite.Business.Services.GroupFeedCommentsServices
{
    public class GroupFeedCommentsService : IGroupFeedCommentsService
    {
        private readonly IGroupFeedCommentsRepository _commentsRepository;

        public GroupFeedCommentsService(IGroupFeedCommentsRepository commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }

        public async Task<GroupFeedCommentsModel> AddCommentAsync(GroupFeedCommentsModel comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment), "Comment cannot be null.");
            }
            return await _commentsRepository.AddCommentAsync(comment);
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            if (commentId <= 0)
            {
                throw new ArgumentException("Invalid Comment ID.", nameof(commentId));
            }
            await _commentsRepository.DeleteCommentAsync(commentId);
        }

        public async Task<GroupFeedCommentsModel> GetCommentByIdAsync(int commentId)
        {
            if (commentId <= 0)
            {
                throw new ArgumentException("Invalid Comment ID.", nameof(commentId));
            }
            var comment = await _commentsRepository.GetCommentByIdAsync(commentId);
            if (comment == null)
            {
                throw new KeyNotFoundException("Comment not found.");
            }
            return comment;
        }

        public async Task<IEnumerable<GroupFeedCommentsModel>> GetCommentsByPostIdAsync(
            int groupPostId
        )
        {
            if (groupPostId <= 0)
            {
                throw new ArgumentException("Invalid Group Post ID.", nameof(groupPostId));
            }
            return await _commentsRepository.GetCommentsByPostIdAsync(groupPostId);
        }

        public async Task UpdateCommentAsync(GroupFeedCommentsModel comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment), "Comment cannot be null.");
            }
            await _commentsRepository.UpdateCommentAsync(comment);
        }
    }
}
