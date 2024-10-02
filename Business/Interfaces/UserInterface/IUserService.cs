using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SphereWebsite.Data.Models;

namespace SphereWebsite.Business.Interfaces.UserInterface
{
    public interface IUserService
    {
        Task<UserModel> RegisterUser(UserModel user);
        Task<UserModel> Login(string email, string password, HttpContext httpContext);
        Task Logout(HttpContext httpContext);
        Task<UserModel> GetUserById(int userId);
        
    }

}