using SphereWebsite.Business.Interfaces.UserInterface;
using SphereWebsite.Data.Interfaces.UserInterface;
using SphereWebsite.Data.Models;
using SphereWebsite.Data.Repositories;
using System.Threading.Tasks;

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

            user.Password = HashPassword(user.Password);
            return await _userRepository.Register(user);
        }

        public async Task<UserModel> Login(string email, string password)
        {
            var hashedPassword = HashPassword(password);
            return await _userRepository.Authenticate(email, hashedPassword);
        }

        private string HashPassword(string password)
        {
            return password; 
        }
    }
}
