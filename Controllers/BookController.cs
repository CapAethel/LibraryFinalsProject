using LibraryFinalsProject.Models;
using LibraryFinalsProject.Services.Interface;
using LibraryFinalsProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryFinalsProject.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: /Book/Index
        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllBooksAsync();
            var bookViewModels = books.Select(book => new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                CategoryId = book.CategoryId,
                // Map other properties as needed
            }).ToList();

            ViewBag.Categories = await _bookService.GetAllCategoriesAsync();
            return View(bookViewModels);
        }
        public async Task<IActionResult> Index2()
        {
            var books = await _bookService.GetAllBooksAsync();
            var bookViewModels = books.Select(book => new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                CategoryId = book.CategoryId,
                // Map other properties as needed
            }).ToList();

            ViewBag.Categories = await _bookService.GetAllCategoriesAsync();
            return View(bookViewModels);
        }


        // GET: /Book/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _bookService.GetAllCategoriesAsync();
            return View("Create", new BookViewModel());
        }

        // POST: /Book/Create
        [HttpPost]
        public async Task<IActionResult> Create(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var book = new Book
                {
                    Title = bookViewModel.Title,
                    Author = bookViewModel.Author,
                    CategoryId = bookViewModel.CategoryId,
                    // Map other properties as needed
                };

                await _bookService.CreateBookAsync(book);
                return RedirectToAction("Index");
            }

            ViewBag.Categories = await _bookService.GetAllCategoriesAsync();
            return View("Create", bookViewModel);
        }

        // GET: /Book/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var bookViewModel = new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                CategoryId = book.CategoryId,
                // Map other properties as needed
            };

            ViewBag.Categories = await _bookService.GetAllCategoriesAsync();
            return View("Edit", bookViewModel);
        }

        // POST: /Book/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var book = new Book
                {
                    Id = bookViewModel.Id,
                    Title = bookViewModel.Title,
                    Author = bookViewModel.Author,
                    CategoryId = bookViewModel.CategoryId,
                    // Map other properties as needed
                };

                try
                {
                    await _bookService.UpdateBookAsync(book);
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _bookService.GetBookByIdAsync(book.Id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.Categories = await _bookService.GetAllCategoriesAsync();
            return View("Edit", bookViewModel);
        }

        // POST: /Book/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return RedirectToAction("Index");
        }

        // GET: /Book/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var category = await _bookService.GetBookByIdAsync(book.CategoryId);

            var bookViewModel = new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                CategoryId = book.CategoryId,
                CategoryName = category?.CategoryName, 
                BookDescription = book.BookDescription 
            };

            return View("Details", bookViewModel);
        }
    }
}
