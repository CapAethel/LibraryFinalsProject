using LibraryFinalsProject.Data;
using LibraryFinalsProject.Models;
using LibraryFinalsProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class BookController : Controller
{
    private readonly ApplicationDbContext _context;

    public BookController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var books = await _context.Books
            .Include(b => b.Category)
            .Select(b => new BookViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                CategoryName = b.Category.CategoryName
            })
            .ToListAsync();

        ViewBag.Categories = await _context.Categories.ToListAsync();
        return View(books);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _context.Categories.ToListAsync();
        return PartialView("_CreateEdit", new Book());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Add(book);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        ViewBag.Categories = await _context.Categories.ToListAsync();
        return PartialView("_CreateEdit", book);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _context.Books
            .Include(b => b.Category)
            .Where(b => b.Id == id)
            .Select(b => new BookViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                CategoryId = b.CategoryId // Assuming you have CategoryId in BookViewModel
            })
            .FirstOrDefaultAsync();

        if (book == null)
        {
            return Json(new { success = false });
        }

        ViewBag.Categories = await _context.Categories.ToListAsync();
        return PartialView("_CreateEdit", book);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Book book)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(book);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Books.Any(e => e.Id == book.Id))
                {
                    return Json(new { success = false });
                }
                else
                {
                    throw;
                }
            }
        }

        ViewBag.Categories = await _context.Categories.ToListAsync();
        return PartialView("_CreateEdit", book);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return Json(new { success = false });
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return Json(new { success = true });
    }
}
