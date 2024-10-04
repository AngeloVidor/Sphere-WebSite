using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SphereWebSite.Business.Interfaces.GroupInterface;
using SphereWebSite.Data.Models.Group;

namespace SphereWebSite.Presentation.Controllers.GroupsController
{
    public class GroupsController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly ILogger<GroupsController> _logger;

        public GroupsController(IGroupService groupService, ILogger<GroupsController> logger)
        {
            _groupService = groupService;
            _logger = logger;
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
        public async Task<IActionResult> Create(GroupModel group)
        {
            _logger.LogInformation("Iniciando o processo de criação do grupo.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "ModelState inválido ao tentar criar o grupo: {@GroupModel}",
                    group
                );
                return View(group);
            }

            try
            {
                var createdGroup = await _groupService.CreateGroup(group);
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
    }
}
