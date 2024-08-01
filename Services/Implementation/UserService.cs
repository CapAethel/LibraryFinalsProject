using LibraryFinalsProject.Data.Repositories;
using LibraryFinalsProject.Models;
using LibraryFinalsProject.Services.Interface;

namespace LibraryFinalsProject.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByNameAndPasswordAsync(string name, string password)
        {
            return await _userRepository.GetUserByNameAndPasswordAsync(name, password);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task CreateUserAsync(User user)
        {
            await _userRepository.CreateAsync(user);
        }
    }
}
