using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using WebApplication909.Areas.Admin.Interfaces;
using WebApplication909.Models;

namespace WebApplication909.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IRolesRepository _rolesRepository;


        public AccountController(IUsersRepository usersRepository, IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
            _usersRepository = usersRepository;
        }

        public IActionResult Authorization()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authorization(Authorization authorization)
        {
            if (authorization.Login == authorization.Password)
            {
                ModelState.AddModelError("",
                    "Имя и пароль не должны совпадать");
            }

            var existingUser = _usersRepository.TryGetByLogin(authorization.Login);

            if (existingUser == null)
            {
                ModelState.AddModelError("", "Такого пользователя не существует!\r\nПройдите регистрацию!");
            }

            if (authorization.Password != existingUser?.Password)
            {
                ModelState.AddModelError("", "Неправильный пароль пользователя!");
            }

            if (!ModelState.IsValid)
            {
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

            var existingUser = _usersRepository.TryGetByLogin(registration.Login);

            if (existingUser != null)
            {
                ModelState.AddModelError("", "Пользователь с таким логином уже зарегистрирован!\r\n" +
                    "Необходимо зарегистрироваться под другим логином!");
            }

            if (!ModelState.IsValid)
            {
                return View(registration);
            }

            var user = new User()
            {
                Login = registration.Login,
                Password = registration.Password,
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                Phone = registration.Phone,
            };

            _usersRepository.Add(user);

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
