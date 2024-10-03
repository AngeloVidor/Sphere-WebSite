using System.Collections.Generic;
using System.Threading.Tasks;
using SphereWebsite.Data.Models;

namespace SphereWebsite.Data.Interfaces.PostsServiceInterface
{
    public interface IPostsService
    {
        Task<PostsModel> CreatePost(
            PostsModel post,
            IFormFile? image = null,
            string[]? selectedTags = null
        );
        Task<PostsModel> GetPostById(int postId);
        Task<IEnumerable<PostsModel>> GetAllPosts();
        Task<PostsModel> UpdatePost(PostsModel post, IFormFile? image = null);
        Task DeletePost(int postId);
    }
}
