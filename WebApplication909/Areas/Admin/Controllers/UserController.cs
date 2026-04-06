using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication909.Areas.Admin.Interfaces;
using WebApplication909.Areas.Admin.Models;
using WebApplication909.Models;

namespace WebApplication909.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IRolesRepository _rolesRepository;

        public UserController(IUsersRepository usersRepository, IRolesRepository rolesRepository)
        {
            _usersRepository = usersRepository;
            _rolesRepository = rolesRepository;
        }

        public IActionResult Index()
        {
            var users = _usersRepository.GetAll();
            return View(users);
        }

        public IActionResult Delete(Guid id)
        {
            _usersRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Detail(Guid id)
        {
            var user = _usersRepository.TryGetById(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            if (_usersRepository.TryGetByLogin(user.Login) != null)
            {
                ModelState.AddModelError("Login", "Пользователь с таким логином уже существует");
                return View(user);
            }

            _usersRepository.Add(user);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(Guid id)
        {
            var user = _usersRepository.TryGetById(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public IActionResult Update(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var existingUser = _usersRepository.TryGetByLogin(user.Login);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                ModelState.AddModelError("Login", "Пользователь с таким логином уже существует");
                return View(user);
            }

            _usersRepository.Update(user);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ChangePassword(Guid id)
        {
            var existingUser = _usersRepository.TryGetById(id);
            if (existingUser == null) return NotFound();

            var changePassword = new ChangePassword
            {
                Login = existingUser.Login
            };
            return View(changePassword);
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePassword changePassword)
        {
            if (changePassword.Login == changePassword.Password)
                ModelState.AddModelError("", "Имя и пароль не должны совпадать");

            if (!ModelState.IsValid)
                return View(changePassword);

            _usersRepository.ChangePassword(changePassword.Login, changePassword.Password);
            var user = _usersRepository.TryGetByLogin(changePassword.Login);
            if (user == null) return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Detail), new { id = user.Id });
        }

        [HttpGet]
        public IActionResult ChangeRole(Guid id)
        {
            var existingUser = _usersRepository.TryGetById(id);
            if (existingUser == null) return NotFound();

            var changeRole = new ChangeRole
            {
                Login = existingUser.Login,
                Role = existingUser.Role?.Name,
                Roles = _rolesRepository.GetAll().Select(role => new SelectListItem
                {
                    Value = role.Name,
                    Text = role.Name
                }).ToList()
            };
            return View(changeRole);
        }

        [HttpPost]
        public IActionResult ChangeRole(ChangeRole changeRole)
        {
            if (!ModelState.IsValid)
                return View(changeRole);

            var newRole = _rolesRepository.TryGetByName(changeRole.Role);
            _usersRepository.ChangeRole(changeRole.Login, newRole);

            var user = _usersRepository.TryGetByLogin(changeRole.Login);
            if (user == null) return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Detail), new { id = user.Id });
        }
    }
}