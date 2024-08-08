using LibraryFinalsProject.Data.Interfaces;
using LibraryFinalsProject.Data.Repositories;
using LibraryFinalsProject.Models;
using LibraryFinalsProject.Services.Interface;
using LibraryFinalsProject.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryFinalsProject.Services.Implementation
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BookService(IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<BookViewModel>> GetAllBooksAsync()
        {
            var books = await _bookRepository.GetAllWithCategoryAsync();

            return books.Select(b => new BookViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                CategoryName = b.Category.CategoryName
            }).ToList();
        }

        public async Task<BookViewModel> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdWithCategoryAsync(id);

            if (book == null)
                return null;

            return new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                CategoryId = book.CategoryId,
                CategoryName = book.Category.CategoryName, // Ensure CategoryName is included
                BookDescription = book.BookDescription // Ensure BookDescription is included if you have it
            };
        }

        public async Task CreateBookAsync(Book book)
        {
            await _bookRepository.CreateAsync(book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            await _bookRepository.UpdateAsync(book);
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book != null)
            {
                await _bookRepository.DeleteAsync(book);
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<List<BookViewModel>> GetLatestBooksAsync(int count)
        {
            var books = await _bookRepository.GetAllAsync(); // Ensure your repository method supports sorting

            var latestBooks = books
                .OrderByDescending(b => b.Title) // Replace with your sorting criteria
                .Take(count)
                .Select(b => new BookViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    CategoryName = b.Category?.CategoryName,
                    BookDescription = b.BookDescription // Ensure this property is mapped
                })
                .ToList();

            return latestBooks;
        }
    }
}
