using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication909.Areas.Admin.Interfaces;
using WebApplication909.Models;

namespace WebApplication909.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public AccountController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Authorization(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Authorization(Authorization model, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _usersRepository.TryGetByLogin(model.Login);
            if (user == null)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(model);
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(model);
            }

            await SignInAsync(user, model.IsRememberMe);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Registration(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registration(Registration model, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            if (_usersRepository.TryGetByLogin(model.Login) != null)
            {
                ModelState.AddModelError("Login", "Пользователь с таким логином уже существует");
                return View(model);
            }

            var user = new User
            {
                Login = model.Login,
                PasswordHash = _passwordHasher.HashPassword(null, model.Password),
                Phone = model.Phone,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreationDateTime = DateTime.Now
            };
            _usersRepository.Add(user);

            // Автоматический вход после регистрации
            await SignInAsync(user, isPersistent: false);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInAsync(User user, bool isPersistent)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Email, user.Login)
            };
            // Если у пользователя есть роль, добавляем claim
            if (user.Role != null)
                claims.Add(new Claim(ClaimTypes.Role, user.Role.Name));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties { IsPersistent = isPersistent });
        }
    }
}