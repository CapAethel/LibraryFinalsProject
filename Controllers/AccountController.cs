using LibraryFinalsProject.Models;
using LibraryFinalsProject.Services.Interface;
using LibraryFinalsProject.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
                        new Claim(ClaimTypes.Role, user.RoleId == 1 ? "User" : "Admin")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    // Redirect based on role
                    return RedirectToAction(user.RoleId == 2 ? "Index" : "Index2", "Book");
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
            SetupRoleSelectList();
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
                        Password = model.Password, // Ensure you hash passwords in a real application
                        Name = model.Name,
                        RoleId = model.RoleId
                    };

                    await _userService.CreateUserAsync(user);

                    ModelState.Clear();
                    ViewBag.Message = $"{user.Email} is successfully registered. Please proceed to log in.";
                    SetupRoleSelectList();
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Email already registered.");
                }
            }
            SetupRoleSelectList();
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

        private void SetupRoleSelectList()
        {
            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "User" },
                new SelectListItem { Value = "2", Text = "Admin" }
            };
        }
    }
}
