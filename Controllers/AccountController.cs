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
using LibraryFinalsProject.Services.Interface;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryFinalsProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
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
                var user = await _userService.GetUserByNameAndPasswordAsync(model.Name, model.Password);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("Name", user.Name),
                        new Claim(ClaimTypes.Role, user.RoleId == 1 ? "User" : "Admin"),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    // Redirect based on role
                    if (user.RoleId == 2) // Admin
                    {
                        return RedirectToAction("Index", "Book");
                    }
                    else // User
                    {
                        return RedirectToAction("UserIndex", "Book");
                    }
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
            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "User" },
                new SelectListItem { Value = "2", Text = "Admin" }
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userService.GetUserByEmailAsync(model.Email);
                if (existingUser == null)
                {
                    var user = new User
                    {
                        Email = model.Email,
                        Password = model.Password,
                        Name = model.Name,
                        RoleId = model.RoleId
                    };

                    await _userService.CreateUserAsync(user);

                    ModelState.Clear();
                    ViewBag.Message = $"{user.Email} is successfully registered. Please proceed to log in.";
                    ViewBag.Roles = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "1", Text = "User" },
                        new SelectListItem { Value = "2", Text = "Admin" }
                    };
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Email already registered.");
                }
            }
            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "User" },
                new SelectListItem { Value = "2", Text = "Admin" }
            };
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [Authorize]
        public IActionResult SecurePage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }
    }
}
