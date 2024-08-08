using LibraryFinalsProject.Models;
using LibraryFinalsProject.ViewModels;

namespace LibraryFinalsProject.Services.Interface
{
    public interface IBookService
    {
        Task<IEnumerable<BookViewModel>> GetAllBooksAsync();
        Task<BookViewModel> GetBookByIdAsync(int id);
        Task CreateBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<List<BookViewModel>> GetLatestBooksAsync(int count);
    }
}
