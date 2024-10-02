using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SphereWebsite.Business.Interfaces.S3Interface;
using SphereWebsite.Business.Interfaces.UserInterface;
using SphereWebsite.Data.Models;

namespace SphereWebsite.UI.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IS3Service _s3Service;

        public UsersController(IUserService userService, IS3Service s3Service)
        {
            _userService = userService;
            _s3Service = s3Service;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel user, IFormFile ProfileImage)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    var profileImageUrl = await _s3Service.UploadFileAsync(ProfileImage);
                    user.ProfileImageUrl = profileImageUrl;
                }

                var registeredUser = await _userService.RegisterUser(user);

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Ocorreu um erro ao registrar o usuário: " + ex.Message
                );
                return View(user); 
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Por favor, preencha todos os campos.";
                return View();
            }

            try
            {
                var user = await _userService.Login(email, password, HttpContext);
                if (user != null)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout(HttpContext);
            return RedirectToAction("Login", "Users");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (userId == 0)
            {
                return RedirectToAction("Login");
            }
            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }
            return View(user);
        }
    }
}
