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
        private readonly IUserRepository _userRepository;

        public PostsController(
            IPostsService postsService,
            ICommentsService commentsService,
            IUserRepository userRepository
        )
        {
            _postsService = postsService;
            _commentsService = commentsService;
            _userRepository = userRepository;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Index()
        {
            var posts = await _postsService.GetAllPosts();
            var postsWithUsers = new List<PostWithUserModel>();

            foreach (var post in posts.OrderByDescending(p => p.CreatedAt))
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
        public async Task<IActionResult> Create(PostsModel post, IFormFile? image, string[] selectedTags)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            post.UserId = int.TryParse(userId, out var id) ? id : 0;

            try
            {
                var createdPost = await _postsService.CreatePost(post, image, selectedTags);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(post);
            }
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
            try
            {
                await _postsService.UpdatePost(post, image);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(post);
            }
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
