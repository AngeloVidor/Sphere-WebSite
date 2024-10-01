using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SphereWebsite.Business.Interfaces;
using SphereWebsite.Business.Interfaces.S3Interface;
using SphereWebsite.Data.Interfaces.PostsInterface;
using SphereWebsite.Data.Interfaces.PostsServiceInterface;
using SphereWebsite.Data.Models;

namespace SphereWebsite.Business.Services.PostsServices
{
    public class PostsService : IPostsService
    {
        private readonly IPostRepository _postRepository;
        private readonly IS3Service _s3Service;

        public PostsService(IPostRepository postRepository, IS3Service s3Service)
        {
            _postRepository = postRepository;
            _s3Service = s3Service;
        }

        public async Task<PostsModel> CreatePost(PostsModel post, IFormFile? image = null)
        {
            if (image != null)
            {
                var imageUrl = await _s3Service.UploadFileAsync(image);
                post.ImageUrl = imageUrl;
            }

            return await _postRepository.CreatePost(post);
        }

        public async Task DeletePost(int postId)
        {
            await _postRepository.DeletePost(postId);
        }

        public async Task<IEnumerable<PostsModel>> GetAllPosts()
        {
            return await _postRepository.GetAllPosts();
        }

        public async Task<PostsModel> GetPostById(int postId)
        {
            return await _postRepository.GetPostById(postId);
        }

        public async Task<PostsModel> UpdatePost(PostsModel post)
        {
            return await _postRepository.UpdatePost(post);
        }
    }
}
