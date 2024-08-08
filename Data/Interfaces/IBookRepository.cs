using LibraryFinalsProject.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace LibraryFinalsProject.Data.Repositories
{

    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetAllWithCategoryAsync();
        Task<IEnumerable<Book>> FindWithCategoryAsync(Func<Book, bool> predicate);
        Task<Book> GetByIdWithCategoryAsync(int id);
        Task<IEnumerable<Book>> GetAllAsync();
    }
}