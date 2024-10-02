using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SphereWebsite.Business.Interfaces.UserInterface;
using SphereWebsite.Data.Models;

namespace SphereWebsite.UI.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel user)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }
                return View(user);
            }

            try
            {
                var registeredUser = await _userService.RegisterUser(user);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
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
            if(userId == 0)
            {
                return RedirectToAction("Login");
            }
            var user = await _userService.GetUserById(userId);
            if(user == null)
            {
                return NotFound("Usuário não encontrado.");
            }
            return View(user);
        }
    }
}
