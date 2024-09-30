using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using SphereWebsite.Business.Interfaces.UserInterface;
using SphereWebsite.Data.Interfaces.UserInterface;
using SphereWebsite.Data.Models;

namespace SphereWebsite.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserModel> RegisterUser(UserModel user)
        {
            var existingUser = await _userRepository.GetUserByEmail(user.Email);
            if (existingUser != null)
            {
                throw new Exception("Email já está registrado");
            }

            var existingNickNameUser = await _userRepository.GetUserByNickName(user.NickName);
            if(existingNickNameUser != null)
            {
                throw new Exception("NickName já está em uso");
            }

            user.Password = HashPassword(user.Password);
            return await _userRepository.Register(user);
        }

        public async Task<UserModel> Login(string email, string password, HttpContext httpContext)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            if (!VerifyPassword(password, user.Password))
            {
                throw new Exception("Senha inválida");
            }

            await SignInUser(user, httpContext);

            return user;
        }

        public async Task SignInUser(UserModel user, HttpContext httpContext)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public async Task Logout(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
