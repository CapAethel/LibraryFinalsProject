using LibraryFinalsProject.Data;
using LibraryFinalsProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace LibraryFinalsProject.Data.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Book>> FindWithCategoryAsync(Func<Book, bool> predicate)
        {
            return await Task.Run(() => _context.Books
                .Include(a => a.Category)
                .Where(predicate));
        }

        public async Task<IEnumerable<Book>> GetAllWithCategoryAsync()
        {
            return await _context.Books.Include(a => a.Category).ToListAsync();
        }

        public async Task<Book> GetByIdWithCategoryAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}