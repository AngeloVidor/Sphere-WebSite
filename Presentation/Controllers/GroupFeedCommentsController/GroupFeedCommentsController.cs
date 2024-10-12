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
        
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdString, out int userId))
            {
                _logger.LogInformation("Usuário ID: {UserId}", userId);

                if (string.IsNullOrWhiteSpace(content))
                {
                    ModelState.AddModelError(
                        nameof(content),
                        "O conteúdo do comentário é obrigatório."
                    );
                }

                if (groupPostID <= 0)
                {
                    ModelState.AddModelError(
                        nameof(groupPostID),
                        "O ID do post do grupo é obrigatório."
                    );
                }

                if (ModelState.IsValid)
                {
                    var newComment = new GroupFeedCommentsModel
                    {
                        UserID = userId,
                        GroupPostID = groupPostID,
                        Content = content,
                        CreatedAt = DateTime.Now 
                    };

                    await _commentsService.AddCommentAsync(newComment);
                    return RedirectToAction("Details", "GroupPosts", new { id = groupPostID });
                }
                else
                {
                    _logger.LogWarning("ModelState inválido: {@ModelState}", ModelState);
                }
            }
            else
            {
                _logger.LogError("Falha ao obter UserId a partir dos claims.");
            }

            var post = await _groupPostsService.GetPostByIdAsync(groupPostID);
            var comments = await _commentsService.GetCommentsByPostIdAsync(groupPostID);

            var viewModel = new GroupPostCommentsViewModel
            {
                GroupPost = post,
                Comments = comments.ToList()
            };

            return View("~/Views/Groups/GroupPosts/GroupComments/Create.cshtml", viewModel);
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

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _commentsService.GetCommentByIdAsync(id);
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
