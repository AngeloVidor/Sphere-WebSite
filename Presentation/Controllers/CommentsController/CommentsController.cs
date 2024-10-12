using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SphereWebsite.Business.Interfaces.CommentsInterface;
using SphereWebSite.Business.Interfaces.GroupPostsInterface;
using SphereWebsite.Data.Models;
using SphereWebSite.Data.Models.GroupFeedComments;

namespace SphereWebsite.Presentation.Controllers.CommentsController
{
    public class CommentsController : Controller
    {
        private readonly ICommentsService _commentsService;
        private readonly IGroupPostsService _groupPostsService;

        public CommentsController(
            IGroupPostsService groupPostsService,
            ICommentsService commentsService
        )
        {
            _commentsService = commentsService;
            _groupPostsService = groupPostsService;
        }

        [HttpGet]
        public IActionResult Create(int postId)
        {
            var comment = new CommentsModel { PostID = postId };
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentsModel comment)
        {
            if (ModelState.IsValid)
            {
                await _commentsService.AddComment(comment);
                return RedirectToAction("Details", "Posts", new { id = comment.PostID });
            }
            return View(comment);
        }

        [HttpGet]
        public async Task<IActionResult> Index(int postId)
        {
            var comments = await _commentsService.GetCommentByPostId(postId);
            return View(comments);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var comment = await _commentsService.GetCommentById(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CommentsModel comment)
        {
            if (ModelState.IsValid)
            {
                await _commentsService.UpdateComment(comment);
                return RedirectToAction("Details", "Posts", new { id = comment.PostID });
            }
            return View(comment);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _commentsService.GetCommentById(id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _commentsService.GetCommentById(id);
            if (comment != null)
            {
                await _commentsService.DeleteComment(id);
                return RedirectToAction("Details", "Posts", new { id = comment.PostID });
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int postId, string content)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdString, out int userId))
            {
                var comment = new CommentsModel
                {
                    PostID = postId,
                    Content = content,
                    UserID = userId,
                    CreatedAt = DateTime.Now
                };

                if (ModelState.IsValid)
                {
                    await _commentsService.AddComment(comment);
                    return Ok(
                        new { success = true, message = "Comentário adicionado com sucesso!" }
                    );
                }
            }

            return BadRequest(new { success = false, message = "Erro ao adicionar comentário." });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var comment = await _commentsService.GetCommentById(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            await _commentsService.DeleteComment(commentId);
            return Ok(new { success = true, message = "Comentário excluído com sucesso!" });
        }
    }
}
