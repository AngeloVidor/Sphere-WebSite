using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SphereWebsite.Data.Models;

namespace SphereWebsite.Business.Interfaces.CommentsInterface
{
    public interface ICommentsService
    {
        Task<CommentsModel> AddComment(CommentsModel comments);
        Task<CommentsModel> GetCommentById(int commentId);
        Task<IEnumerable<CommentsModel>> GetCommentByPostId(int postId);
        Task UpdateComment(CommentsModel comment);
        Task DeleteComment(int commentId);
    }
}
