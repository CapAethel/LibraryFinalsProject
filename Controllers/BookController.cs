using LibraryFinalsProject.Data;
using LibraryFinalsProject.Models;
using LibraryFinalsProject.Services.Interface;
using LibraryFinalsProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace LibraryFinalsProject.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _bookService.GetAllBooksAsync();
            ViewBag.Categories = await _bookService.GetAllCategoriesAsync();
            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _bookService.GetAllCategoriesAsync();
            return PartialView("Index", new Book());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                await _bookService.CreateBookAsync(book);
                return Json(new { success = true });
            }

            ViewBag.Categories = await _bookService.GetAllCategoriesAsync();
            return PartialView("Index", book);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var bookViewModel = await _bookService.GetBookByIdAsync(id);

            if (bookViewModel == null)
            {
                return Json(new { success = false });
            }

            ViewBag.Categories = await _bookService.GetAllCategoriesAsync();
            return PartialView("Index", bookViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _bookService.UpdateBookAsync(book);
                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _bookService.GetBookByIdAsync(book.Id) == null)
                    {
                        return Json(new { success = false });
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.Categories = await _bookService.GetAllCategoriesAsync();
            return PartialView("Index", book);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return Json(new { success = true });
        }
    }
}