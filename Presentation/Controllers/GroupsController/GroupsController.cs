using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SphereWebSite.Business.Interfaces.GroupInterface;
using SphereWebsite.Business.Interfaces.S3Interface;
using SphereWebSite.Data.Models.Group;

namespace SphereWebSite.Presentation.Controllers.GroupsController
{
    public class GroupsController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly ILogger<GroupsController> _logger;
        private readonly IS3Service _s3Service;

        public GroupsController(
            IGroupService groupService,
            ILogger<GroupsController> logger,
            IS3Service s3Service
        )
        {
            _groupService = groupService;
            _logger = logger;
            _s3Service = s3Service;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var groups = await _groupService.GetAllGroups();
            return View(groups);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GroupModel group, IFormFile groupImage)
        {
            _logger.LogInformation("Iniciando o processo de criação do grupo.");

            if (groupImage == null || groupImage.Length == 0)
            {
                ModelState.AddModelError("GroupImageUrl", "A imagem do grupo é obrigatória.");
            }
            else
            {
                try
                {
                    group.GroupImageUrl = await _s3Service.UploadFileAsync(groupImage);

                    ModelState.Clear();
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Erro ao fazer upload da imagem do grupo: {@GroupModel}",
                        group
                    );
                    ModelState.AddModelError(
                        "GroupImageUrl",
                        "Erro ao fazer upload da imagem do grupo."
                    );
                }
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage))
                    .ToList();

                _logger.LogWarning(
                    "ModelState inválido ao tentar criar o grupo: {@GroupModel}. Erros: {@Errors}",
                    group,
                    errors
                );

                return View(group);
            }

            try
            {
                var createdGroup = await _groupService.CreateGroup(group, groupImage);
                _logger.LogInformation("Grupo criado com sucesso: {@CreatedGroup}", createdGroup);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar grupo: {@GroupModel}", group);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(group);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var group = await _groupService.GetGroupById(id);
                if (group == null)
                {
                    return NotFound();
                }
                return View(group);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GroupModel group)
        {
            if (!ModelState.IsValid)
            {
                return View(group);
            }
            try
            {
                var updatedGroup = await _groupService.UpdateGroup(group);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(group);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var group = await _groupService.GetGroupById(id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _groupService.DeleteGroup(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinGroup(int groupId)
        {
            if (groupId <= 0)
            {
                return BadRequest("ID do grupo inválido.");
            }

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                return BadRequest("Usuário não autenticado.");
            }

            try
            {
                await _groupService.JoinGroup(groupId, userId);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
