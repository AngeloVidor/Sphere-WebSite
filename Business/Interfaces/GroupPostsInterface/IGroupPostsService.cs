using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SphereWebsite.Data.Models;

namespace SphereWebSite.Business.Interfaces.GroupPostsInterface
{
    public interface IGroupPostsService
    {
        Task<IEnumerable<GroupPostsModel>> GetAllPostsAsync();
        Task<GroupPostsModel> GetPostByIdAsync(int postId);
        Task<IEnumerable<GroupPostsModel>> GetPostsByGroupIdAsync(int groupId);
        Task AddPostAsync(GroupPostsModel post, IFormFile? file);
        Task UpdatePostAsync(GroupPostsModel post);
        Task DeletePostAsync(int postId);
        Task<bool> AddOrUpdateVoteAsync(int postId, int userId, bool isUpvote);
    }
}
