using System.Collections.Generic;
using System.Threading.Tasks;
using SphereWebSite.Business.Interfaces.GroupPostsInterface;
using SphereWebsite.Business.Interfaces.S3Interface;
using SphereWebsite.Data.Models;
using SphereWebsite.Data.Repositories;

namespace SphereWebSite.Business.Services
{
    public class GroupPostsService : IGroupPostsService
    {
        private readonly IGroupPostsRepository _groupPostsRepository;
        private readonly IS3Service _s3Service;

        public GroupPostsService(IGroupPostsRepository groupPostsRepository, IS3Service s3Service)
        {
            _groupPostsRepository = groupPostsRepository;
            _s3Service = s3Service;
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

        public async Task AddPostAsync(GroupPostsModel post, IFormFile? file)
        {
            if (file != null && file.Length > 0)
            {
                post.ImageUrl = await _s3Service.UploadFileAsync(file);
            }
            else
            {
                post.ImageUrl = null;
            }
            

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
