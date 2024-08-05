using LibraryFinalsProject.Models;

namespace LibraryFinalsProject.Services.Interface
{
    public interface IUserService
    {
        Task<User> GetUserByNameAndPasswordAsync(string name, string password);
        Task<User> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
    }
}
