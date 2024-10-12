using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SphereWebSite.Business.Interfaces.GroupFeedCommentsInterface;
using SphereWebSite.Business.Interfaces.GroupInterface;
using SphereWebSite.Business.Interfaces.GroupPostsInterface;
using SphereWebsite.Business.Interfaces.S3Interface;
using SphereWebsite.Business.Interfaces.UserInterface;
using SphereWebsite.Data.Models;
using SphereWebSite.Data.Models.Group;
using SphereWebSite.Data.Models;

namespace SphereWebSite.Presentation.Controllers
{
    public class GroupPostsController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly IGroupPostsService _groupPostsService;
        private readonly IS3Service _s3Service;
        private readonly ILogger<GroupPostsController> _logger;

        public GroupPostsController(
            IGroupService groupService,
            IGroupPostsService groupPostsService,
            IS3Service s3Service,
            ILogger<GroupPostsController> logger
        )
        {
            _groupService = groupService;
            _groupPostsService = groupPostsService;
            _s3Service = s3Service;
            _logger = logger;
        }

        public async Task<IActionResult> Details(int id)
        {
            var groupModel = await _groupService.GetGroupById(id);

            if (groupModel == null)
            {
                return NotFound();
            }

            groupModel.Posts = (await _groupPostsService.GetPostsByGroupIdAsync(id)).ToList();

            return View("~/Views/Groups/GroupPosts/Details.cshtml", groupModel);
        }

        [HttpGet]
        public IActionResult Create(int groupId)
        {
            ViewBag.GroupId = groupId;
            return View("~/Views/Groups/GroupPosts/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GroupPostsModel post, IFormFile? file)
        {
            _logger.LogInformation("Iniciando o método Create com o post: {@Post}", post);

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(currentUserId))
            {
                _logger.LogWarning("User ID não encontrado. O usuário deve estar autenticado.");
                ModelState.AddModelError("", "Usuário não autenticado.");
                ViewBag.GroupId = post.GroupId;
                return View("~/Views/Groups/GroupPosts/Create.cshtml", post);
            }

            post.UserId = int.Parse(currentUserId);

            if (file != null && file.Length > 0)
            {
                try
                {
                    _logger.LogInformation("Fazendo upload da imagem.");
                    post.ImageUrl = await _s3Service.UploadFileAsync(file);
                    _logger.LogInformation(
                        "Upload da imagem realizado com sucesso: {ImageUrl}",
                        post.ImageUrl
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao fazer upload da imagem.");
                    ModelState.AddModelError("", "Erro ao fazer upload da imagem");
                }
            }
            else
            {
                _logger.LogInformation("Nenhuma imagem foi enviada. A imagem é opcional.");
                post.ImageUrl = null;
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("O modelo é válido. Adicionando post.");
                await _groupPostsService.AddPostAsync(post, file);
                _logger.LogInformation(
                    "Post adicionado com sucesso. Redirecionando para detalhes."
                );
                return RedirectToAction("Details", "GroupPosts", new { id = post.GroupId });
            }

            foreach (var error in ModelState)
            {
                foreach (var errorMessage in error.Value.Errors)
                {
                    _logger.LogWarning(
                        "Erro no campo {Field}: {Message}",
                        error.Key,
                        errorMessage.ErrorMessage
                    );
                }
            }

            _logger.LogWarning("O modelo é inválido. Retornando à view.");
            ViewBag.GroupId = post.GroupId;
            return View("~/Views/Groups/GroupPosts/Create.cshtml", post);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _groupPostsService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GroupPostsModel post)
        {
            if (id != post.GroupPostID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _groupPostsService.UpdatePostAsync(post);
                return RedirectToAction("Details", "GroupPosts", new { id = post.GroupId });
            }
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _groupPostsService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            await _groupPostsService.DeletePostAsync(id);
            return RedirectToAction("Details", "GroupPosts", new { id = post.GroupId });
        }

        
    }
}
