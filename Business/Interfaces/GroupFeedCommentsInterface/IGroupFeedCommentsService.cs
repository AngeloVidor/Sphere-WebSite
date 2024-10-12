using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SphereWebSite.Data.Models.GroupFeedComments;

namespace SphereWebSite.Business.Interfaces.GroupFeedCommentsInterface
{
    public interface IGroupFeedCommentsService
    {
        Task<GroupFeedCommentsModel> AddCommentAsync(GroupFeedCommentsModel comment);
        Task DeleteCommentAsync(int commentId);
        Task<GroupFeedCommentsModel> GetCommentByIdAsync(int commentId);
        Task<IEnumerable<GroupFeedCommentsModel>> GetCommentsByPostIdAsync(int groupPostId);
        Task UpdateCommentAsync(GroupFeedCommentsModel comment);
    }
}
