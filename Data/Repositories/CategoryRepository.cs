
using LibraryFinalsProject.Data.Interfaces;
using LibraryFinalsProject.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryFinalsProject.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}
