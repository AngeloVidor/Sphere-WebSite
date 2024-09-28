using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SphereWebsite.Business.Interfaces.UserInterface;
using SphereWebsite.Business.Services;
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
                Console.WriteLine($"Error for {ModelState.Keys}: {error.ErrorMessage}");
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
            var user = await _userService.Login(email, password);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Email ou senha incorretos.";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
