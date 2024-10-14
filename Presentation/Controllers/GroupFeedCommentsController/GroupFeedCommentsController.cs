using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SphereWebSite.Business.Interfaces.GroupFeedCommentsInterface;
using SphereWebSite.Business.Interfaces.GroupPostsInterface;
using SphereWebSite.Data.Models;
using SphereWebSite.Data.Models.GroupFeedComments;

namespace SphereWebSite.Presentation.Controllers.GroupFeedCommentsController
{
    public class GroupFeedCommentsController : Controller
    {
        private readonly IGroupFeedCommentsService _commentsService;
        private readonly IGroupPostsService _groupPostsService;
        private readonly ILogger<GroupFeedCommentsController> _logger;

        public GroupFeedCommentsController(
            IGroupPostsService groupPostsService,
            IGroupFeedCommentsService commentsService,
            ILogger<GroupFeedCommentsController> logger
        )
        {
            _commentsService = commentsService;
            _groupPostsService = groupPostsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int groupPostId)
        {
            var post = await _groupPostsService.GetPostByIdAsync(groupPostId);

            if (post == null)
            {
                return NotFound();
            }

            var comments = await _commentsService.GetCommentsByPostIdAsync(groupPostId);

            var viewModel = new GroupPostCommentsViewModel
            {
                GroupPost = post,
                Comments = comments.ToList(),
                NewComment = new GroupFeedCommentsModel { GroupPostID = groupPostId }
            };

            return View("~/Views/Groups/GroupPosts/GroupComments/Create.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int groupPostID, string content)
        {
            _logger.LogInformation(
                "Create action started. groupPostID: {GroupPostID}, content: {Content}",
                groupPostID,
                content
            );

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                _logger.LogError(
                    "Error parsing user ID. userIdString: {UserIdString}",
                    userIdString
                );
                return Json(new { success = false, message = "Erro ao identificar o usuário." });
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.LogWarning("Content is empty.");
                return Json(
                    new { success = false, message = "O conteúdo do comentário é obrigatório." }
                );
            }

            if (groupPostID <= 0)
            {
                _logger.LogWarning(
                    "GroupPostID is invalid. groupPostID: {GroupPostID}",
                    groupPostID
                );
                return Json(
                    new { success = false, message = "O ID do post do grupo é obrigatório." }
                );
            }

            var groupPost = await _groupPostsService.GetPostByIdAsync(groupPostID);
            if (groupPost == null)
            {
                _logger.LogWarning("Group post not found. groupPostID: {GroupPostID}", groupPostID);
                return Json(
                    new { success = false, message = "O post do grupo não foi encontrado." }
                );
            }

            var newComment = new GroupFeedCommentsModel
            {
                UserID = userId,
                GroupPostID = groupPostID,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                _logger.LogWarning("Model state is invalid: {Errors}", string.Join(", ", errors));
                return Json(new { success = false, message = "Dados do comentário inválidos." });
            }

            try
            {
                await _commentsService.AddCommentAsync(newComment);
                _logger.LogInformation(
                    "Comment added successfully. groupPostID: {GroupPostID}, userId: {UserID}",
                    groupPostID,
                    userId
                );

                return Json(
                    new
                    {
                        success = true,
                        comment = newComment,
                        message = "Comentário adicionado com sucesso!"
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error adding comment. groupPostID: {GroupPostID}, userId: {UserID}",
                    groupPostID,
                    userId
                );
                return Json(
                    new
                    {
                        success = false,
                        message = "Erro ao adicionar o comentário. Tente novamente."
                    }
                );
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var comment = await _commentsService.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GroupFeedCommentsModel comment)
        {
            if (id != comment.CommentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _commentsService.UpdateCommentAsync(comment);
                return RedirectToAction("Details", "GroupPosts", new { id = comment.GroupPostID });
            }
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            _logger.LogInformation(
                "DeleteComment action started for commentId: {CommentId}",
                commentId
            );

            try
            {
                var comment = await _commentsService.GetCommentByIdAsync(commentId);
                if (comment == null)
                {
                    _logger.LogWarning("Comment not found for commentId: {CommentId}", commentId);
                    return Json(new { success = false, message = "Comentário não encontrado." });
                }

                await _commentsService.DeleteCommentAsync(commentId);
                _logger.LogInformation(
                    "Comment deleted successfully. commentId: {CommentId}",
                    commentId
                );

                return Json(new { success = true, message = "Comentário deletado com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting comment. commentId: {CommentId}", commentId);
                return Json(
                    new
                    {
                        success = false,
                        message = "Erro ao deletar o comentário. Tente novamente."
                    }
                );
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _commentsService.DeleteCommentAsync(id);
            return RedirectToAction("Index", "GroupPosts");
        }

        [HttpGet]
        public async Task<IActionResult> Comments(int groupPostId)
        {
            var groupPost = await _groupPostsService.GetPostByIdAsync(groupPostId);

            if (groupPost == null)
            {
                return NotFound();
            }

            var comments = await _commentsService.GetCommentsByPostIdAsync(groupPostId);

            var viewModel = new GroupPostCommentsViewModel
            {
                GroupPost = groupPost,
                Comments = comments.ToList(),
                NewComment = new GroupFeedCommentsModel()
            };

            return View("~/Views/Groups/GroupPosts/GroupComments/Comments.cshtml", viewModel);
        }
    }
}
