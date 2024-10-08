using System.Collections.Generic;
using System.Threading.Tasks;
using SphereWebSite.Business.Interfaces.GroupPostsInterface;
using SphereWebsite.Data.Models;
using SphereWebsite.Data.Repositories;

namespace SphereWebSite.Business.Services
{
    public class GroupPostsService : IGroupPostsService
    {
        private readonly IGroupPostsRepository _groupPostsRepository;

        public GroupPostsService(IGroupPostsRepository groupPostsRepository)
        {
            _groupPostsRepository = groupPostsRepository;
        }

        public async Task<IEnumerable<GroupPostsModel>> GetAllPostsAsync()
        {
            return await _groupPostsRepository.GetAllPostsAsync();
        }

        public async Task<GroupPostsModel> GetPostByIdAsync(int postId)
        {
            return await _groupPostsRepository.GetPostByIdAsync(postId);
        }

        public async Task<IEnumerable<GroupPostsModel>> GetPostsByGroupIdAsync(int groupId)
        {
            return await _groupPostsRepository.GetPostsByGroupIdAsync(groupId);
        }

        public async Task AddPostAsync(GroupPostsModel post)
        {
            await _groupPostsRepository.AddPostAsync(post);
        }

        public async Task UpdatePostAsync(GroupPostsModel post)
        {
            await _groupPostsRepository.UpdatePostAsync(post);
        }

        public async Task DeletePostAsync(int postId)
        {
            await _groupPostsRepository.DeletePostAsync(postId);
        }
    }
}
