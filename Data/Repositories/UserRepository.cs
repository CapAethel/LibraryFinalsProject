using LibraryFinalsProject.Data;
using LibraryFinalsProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LibraryFinalsProject.Data.Repositories
{

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User> GetUserByNameAndPasswordAsync(string name, string password)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Name == name && x.Password == password);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
        }
    }
}
