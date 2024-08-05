using LibraryFinalsProject.Models;
using LibraryFinalsProject.Services.Interface;
using LibraryFinalsProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LibraryFinalsProject.Controllers
{
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
            ViewBag.Categories = await _bookService.GetAllCategoriesAsync();
            return View(books);
        }

        // GET: /Book/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _bookService.GetAllCategoriesAsync();
            return View(new BookViewModel());
        }

        // POST: /Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var book = new Book
                {
                    Title = bookViewModel.Title,
                    Author = bookViewModel.Author,
                    CategoryId = bookViewModel.CategoryId
                };

                await _bookService.CreateBookAsync(book);
                TempData["SuccessMessage"] = "Book created successfully!";
                return RedirectToAction("Create");
            }

            ViewBag.Categories = await _bookService.GetAllCategoriesAsync();
            return View(bookViewModel);
        }

        // GET: /Book/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var bookViewModel = await _bookService.GetBookByIdAsync(id);

            if (bookViewModel == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _bookService.GetAllCategoriesAsync();
            return View(bookViewModel);
        }

        // POST: /Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var book = new Book
                {
                    Id = bookViewModel.Id,
                    Title = bookViewModel.Title,
                    Author = bookViewModel.Author,
                    CategoryId = bookViewModel.CategoryId
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
            return View(bookViewModel);
        }

        // POST: /Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _bookService.DeleteBookAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception or handle as needed
                Console.WriteLine(ex.Message);
                return RedirectToAction("Index"); // Redirect to Index with error handling
            }
        }

        // GET: /Book/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var bookViewModel = await _bookService.GetBookByIdAsync(id);

            if (bookViewModel == null)
            {
                return NotFound();
            }

            return View(bookViewModel);
        }
    }
}
