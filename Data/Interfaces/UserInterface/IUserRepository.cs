using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SphereWebsite.Data.Models;

namespace SphereWebsite.Data.Interfaces.UserInterface
{
    public interface IUserRepository
    {
        Task<UserModel> Register(UserModel user);
        Task<UserModel> Authenticate(string email, string password);
        Task<UserModel> GetUserByEmail(string email);
        Task<UserModel> GetUserByNickName(string nickName);
    }
}
