using LibraryFinalsProject.Models;
using System.Threading.Tasks;
namespace LibraryFinalsProject.Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByNameAndPasswordAsync(string name, string password);
        Task<User> GetUserByEmailAsync(string email);
    }
}