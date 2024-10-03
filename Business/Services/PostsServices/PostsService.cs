using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        public async Task<PostsModel> CreatePost(PostsModel post, IFormFile? image = null, string[]? selectedTags = null)
        {
            if (string.IsNullOrEmpty(post.UserId.ToString()) || post.UserId <= 0)
            {
                throw new ArgumentException("User ID is required and must be valid.");
            }

            if (selectedTags == null || !selectedTags.Any())
            {
                throw new ArgumentException("You must select at least one tag.");
            }

            if (selectedTags.Length > 3)
            {
                throw new ArgumentException("You can select up to 3 tags.");
            }

            post.SelectedTags = selectedTags.ToList();

            if (image != null && image.Length > 0)
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

        public async Task<PostsModel> UpdatePost(PostsModel post, IFormFile? image = null)
        {
            var existingPost = await _postRepository.GetPostById(post.PostID);
            if (existingPost == null)
            {
                throw new KeyNotFoundException("Post not found.");
            }

            if (image != null && image.Length > 0)
            {
                var imageUrl = await _s3Service.UploadFileAsync(image);
                existingPost.ImageUrl = imageUrl;
            }

            existingPost.Title = post.Title;
            existingPost.Description = post.Description;
            existingPost.UserId = post.UserId;

            return await _postRepository.UpdatePost(existingPost);
        }
    }
}
