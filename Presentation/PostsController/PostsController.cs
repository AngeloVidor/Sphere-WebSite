using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SphereWebsite.Business.Interfaces.CommentsInterface;
using SphereWebsite.Business.Interfaces.S3Interface;
using SphereWebsite.Data.Interfaces.PostsServiceInterface;
using SphereWebsite.Data.Interfaces.UserInterface;
using SphereWebsite.Data.Models;
using SphereWebsite.Data.Models.PostsWithComments;

namespace SphereWebsite.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostsService _postsService;
        private readonly ICommentsService _commentsService;
        private readonly IS3Service _s3Service;
        private readonly IUserRepository _userRepository;

        public PostsController(
            IPostsService postsService,
            ICommentsService commentsService,
            IUserRepository userRepository,
            IS3Service s3Service
        )
        {
            _postsService = postsService;
            _commentsService = commentsService;
            _userRepository = userRepository;
            _s3Service = s3Service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Index()
        {
            var posts = await _postsService.GetAllPosts();
            var postsWithUsers = new List<PostWithUserModel>();

            foreach (var post in posts.OrderByDescending(p =>  p.CreatedAt))
            {
                var user = await _userRepository.GetUserById(post.UserId);
                postsWithUsers.Add(new PostWithUserModel { Post = post, User = user });
            }

            return View(postsWithUsers);
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details(int id)
        {
            var post = await _postsService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }

            var comments = await _commentsService.GetCommentByPostId(id);

            var viewModel = new PostsWithCommentsViewModel
            {
                Post = post,
                Comments = comments.ToList()
            };

            return View(viewModel);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostsModel post, IFormFile? image)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            post.UserId = int.TryParse(userId, out var id) ? id : 0;

            Console.WriteLine($"User ID Obtido: {userId}");
            Console.WriteLine($"User ID Convertido: {post.UserId}");

            if (post.UserId == 0)
            {
                ModelState.AddModelError("UserId", "User ID is required and must be valid.");
                Console.WriteLine("Erro: User ID não é válido.");
            }

            if (ModelState.IsValid)
            {
                await _postsService.CreatePost(post, image);
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Erro: {error.ErrorMessage}");
            }

            return View(post);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _postsService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostsModel post, IFormFile? image)
        {
            Console.WriteLine(
                $"Edit called with ID: {id}, Title: {post.Title}, Description: {post.Description}, UserId: {post.UserId}"
            );
            var existingPost = await _postsService.GetPostById(id);
            if (existingPost == null)
            {
                Console.WriteLine("Post not found.");
                return NotFound();
            }

            if (image != null && image.Length > 0)
            {
                Console.WriteLine($"Imagem recebida: {image.FileName}, Tamanho: {image.Length}");

                var imageUrl = await _s3Service.UploadFileAsync(image);
                existingPost.ImageUrl = imageUrl;
            }
            else
            {
                Console.WriteLine("A imagem está null ou vazia.");
            }

            existingPost.Title = post.Title;
            existingPost.Description = post.Description;
            existingPost.UserId = post.UserId;

            await _postsService.UpdatePost(existingPost);
            Console.WriteLine("Post atualizado com sucesso.");

            return RedirectToAction("Index");
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postsService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _postsService.DeletePost(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
