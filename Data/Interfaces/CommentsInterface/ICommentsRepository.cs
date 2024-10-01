using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SphereWebsite.Data.Models;

namespace SphereWebsite.Data.Interfaces.CommentsInterface
{
    public interface ICommentsRepository
    {
        Task<CommentsModel> AddComment(CommentsModel comments);
        Task<CommentsModel> GetCommentById(int commentId);
        Task<IEnumerable<CommentsModel>> GetCommentsByPostId(int postId);
        Task UpdateComment(CommentsModel comment);
        Task DeleteComment(int commentId); 

    }
}
