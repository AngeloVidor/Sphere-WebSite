using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SphereWebsite.Data.ApplicationContext;
using SphereWebsite.Data.Interfaces.UserInterface;
using SphereWebsite.Data.Models;

namespace SphereWebsite.Data.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserModel> GetUserById(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<UserModel> Authenticate(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u =>
                u.Email == email && u.Password == password
            );
        }

        public async Task<UserModel> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserModel> Register(UserModel user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserModel> GetUserByNickName(string nickName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.NickName == nickName);
        }
    }
}
