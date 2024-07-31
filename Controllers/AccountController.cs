using LibraryFinalsProject.Data;
using LibraryFinalsProject.Models;
using LibraryFinalsProject.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = _context.Users.SingleOrDefault(x => x.Name == model.Name && x.Password == model.Password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("Name", user.Name),
                    new Claim(ClaimTypes.Role, "User"),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    // Allow refresh token
                    IsPersistent = model.RememberMe // RememberMe checkbox on LoginViewModel
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToAction("SecurePage");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
            }
        }
        return View(model);
    }

    public IActionResult Registration()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Registration(RegistrationViewModel model)
    {
        if (ModelState.IsValid)
        {
            var existingUser = _context.Users.SingleOrDefault(x => x.Email == model.Email);
            if (existingUser == null)
            {
                var user = new User
                {
                    Email = model.Email,
                    Password = model.Password,
                    Name = model.Name
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                ModelState.Clear();
                ViewBag.Message = $"{user.Email} is successfully registered. Please proceed to log in.";
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Email already registered.");
            }
        }
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index");
    }

    [Authorize]
    public IActionResult SecurePage()
    {
        ViewBag.Name = HttpContext.User.Identity.Name;
        return View();
    }
}
