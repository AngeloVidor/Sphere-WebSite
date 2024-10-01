using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SphereWebsite.Business.Interfaces.CommentsInterface;
using SphereWebsite.Data.Interfaces.PostsServiceInterface;
using SphereWebsite.Data.Models;
using SphereWebsite.Data.Models.PostsWithComments;

namespace SphereWebsite.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostsService _postsService;
        private readonly ICommentsService _commentsService;

        public PostsController(IPostsService postsService, ICommentsService commentsService)
        {
            _postsService = postsService;
            _commentsService = commentsService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Index()
        {
            var posts = await _postsService.GetAllPosts();
            return View(posts);
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
        public async Task<IActionResult> Create(PostsModel post)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            post.UserId = int.TryParse(userId, out var id) ? id : 0;

            Console.WriteLine(
                $"Title: {post.Title}, Description: {post.Description}, UserId: {post.UserId}"
            );

            if (post.UserId == 0)
            {
                ModelState.AddModelError("UserId", "User ID is required and must be valid.");
            }

            if (ModelState.IsValid)
            {
                await _postsService.CreatePost(post);
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Erro: {error.ErrorMessage}");
            }

            return View(post);
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _postsService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostsModel post)
        {
            if (id != post.PostID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _postsService.UpdatePost(post);
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postsService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _postsService.DeletePost(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
