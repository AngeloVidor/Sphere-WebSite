using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SphereWebsite.Data.Interfaces.PostsInterface;
using SphereWebsite.Data.Interfaces.PostsServiceInterface;
using SphereWebsite.Data.Models;

namespace SphereWebsite.Business.Services.PostsServices
{
    public class PostsService : IPostsService
    {
        private readonly IPostRepository _postRepository;
        public PostsService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<PostsModel> CreatePost(PostsModel post)
        {
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