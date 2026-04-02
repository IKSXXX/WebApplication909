using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Models;
using WebApplication909.Models;
using System.Threading.Tasks;

namespace WebApplication909.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Account/Authorization
        public IActionResult Authorization()
        {
            return View();
        }

        // POST: /Account/Authorization
        [HttpPost]
        public async Task<IActionResult> Authorization(AuthorizationViewModel model)
        {
            if (model.Login == model.Password)
            {
                ModelState.AddModelError("", "Логин и пароль не должны совпадать");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Login);
            if (user == null)
            {
                ModelState.AddModelError("", "Пользователь с таким email не найден");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.IsRememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Неверный пароль");
            return View(model);
        }

        // GET: /Account/Registration
        public IActionResult Registration()
        {
            return View();
        }

        // POST: /Account/Registration
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (model.UserName == model.Password)
            {
                ModelState.AddModelError("", "Email и пароль не должны совпадать");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.UserName);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Пользователь с таким email уже зарегистрирован");
                return View(model);
            }

            var user = new User
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Phone
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}