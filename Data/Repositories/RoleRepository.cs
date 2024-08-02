using LibraryFinalsProject.Data.Interfaces;
using LibraryFinalsProject.Models;

namespace LibraryFinalsProject.Data.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
