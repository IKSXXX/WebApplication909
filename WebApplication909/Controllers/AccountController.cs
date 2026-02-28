using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using WebApplication909.Areas.Admin.Interfaces;
using WebApplication909.Models;

namespace WebApplication909.Controllers
{
    public class AccountController(IUsersRepository users) : Controller
    {
        private IUsersRepository _usersRepository = users;
        public IActionResult Authorization()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authorization(Authorization authorization)
        {
            if (authorization.Login == authorization.Password)
            {
                ModelState.AddModelError("", "Логин и пароль не должны совпадать");
            }

            if (!ModelState.IsValid)
            {
                return View(authorization);
            }

            var user = _usersRepository.TryGetByLogin(authorization.Login);
            if (user == null || user.Password != authorization.Password)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(authorization);
            }


            return RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult Registration()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Registration(Registration registration)
        {
            if (registration.Login == registration.Password)
            {
                ModelState.AddModelError("",
                    "Имя и пароль не должны совпадать");
            }

            if (!ModelState.IsValid)
            {
                return View(registration);
            }

            if (_usersRepository.GetAll().FirstOrDefault(u => u.Login == registration.Login) != null)
            {
                ModelState.AddModelError("Login", "Пользователь с таким логином уже существует");
                return View(registration);
            }

            User user = new User
            {
                Login = registration.Login,
                Password = registration.Password
            };

            var users = _usersRepository.GetAll();
            users.Add(user);

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
