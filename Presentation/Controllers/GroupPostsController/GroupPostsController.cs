using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SphereWebSite.Business.Interfaces.GroupInterface;
using SphereWebSite.Business.Interfaces.GroupPostsInterface;
using SphereWebsite.Data.Models;
using SphereWebSite.Data.Models.Group;

namespace SphereWebSite.Presentation.Controllers
{
    public class GroupPostsController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly IGroupPostsService _groupPostsService;

        public GroupPostsController(
            IGroupService groupService,
            IGroupPostsService groupPostsService
        )
        {
            _groupService = groupService;
            _groupPostsService = groupPostsService;
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
        public async Task<IActionResult> Create(GroupPostsModel post)
        {
            if (ModelState.IsValid)
            {
                post.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                await _groupPostsService.AddPostAsync(post);
                return RedirectToAction("Details", "GroupPosts", new { id = post.GroupId });
            }
            return View(post);
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
