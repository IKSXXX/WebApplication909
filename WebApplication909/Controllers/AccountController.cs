using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Models;
using WebApplication909.Models;

namespace WebApplication909.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Authorization(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new Authorization { Login = "", Password = "" });
        }

        [HttpPost]
        public async Task<IActionResult> Authorization(Authorization model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Login);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.IsRememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                        return Redirect(returnUrl ?? "/");
                }
                ModelState.AddModelError("", "Неверный логин или пароль");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpGet]
        public IActionResult Registration(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new Registration
            {
                Login = "",
                Password = "",
                ConfirmPassword = "",
                FirstName = "",
                LastName = "",
                Phone = ""
            });
        }

        [HttpPost]
        public async Task<IActionResult> Registration(Registration model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Login,
                    Email = model.Login,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone = model.Phone,
                    CreationDateTime = DateTime.UtcNow
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Добавляем роль User
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Redirect(returnUrl ?? "/");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}