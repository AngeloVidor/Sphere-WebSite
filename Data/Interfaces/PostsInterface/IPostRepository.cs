using System.Collections.Generic;
using System.Threading.Tasks;
using SphereWebsite.Data.Models;

namespace SphereWebsite.Data.Interfaces.PostsInterface
{
    public interface IPostRepository
    {
        Task<PostsModel> CreatePost(PostsModel post);
        Task<PostsModel> GetPostById(int postId);
        Task<IEnumerable<PostsModel>> GetAllPosts();
        Task DeletePost(int postId);
        Task<PostsModel> UpdatePost(PostsModel post);
    }
}
