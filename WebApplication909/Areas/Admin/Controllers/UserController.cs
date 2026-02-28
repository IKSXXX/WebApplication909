using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication909.Areas.Admin.Interfaces;
using WebApplication909.Models;
using WebApplication909.Areas.Admin.Models;

namespace WebApplication909.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController(IUsersRepository usersRepository, IRolesRepository rolesRepository) : Controller
    {
        IUsersRepository _usersRepository = usersRepository;
        IRolesRepository _rolesRepository = rolesRepository;
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

            return View(user);
        }

        public IActionResult Add(User user)
        {
            _usersRepository.Add(user);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(Guid id)
        {
            var user = _usersRepository.TryGetById(id);
            if (user == null)
                return NotFound();

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

        public IActionResult ChangePassword(Guid id)
        {
            var existingUser = _usersRepository.TryGetById(id);

            var changePassword = new ChangePassword()
            {
                Login = existingUser?.Login
            };

            return View(changePassword);
        }


        [HttpPost]
        public IActionResult ChangePassword(ChangePassword changePassword)
        {
            if (changePassword.Login == changePassword.Password)
            {
                ModelState.AddModelError("",
                    "Имя и пароль не должны совпадать");
            }

            if (!ModelState.IsValid)
            {
                return View(changePassword);
            }

            _usersRepository.ChangePassword(changePassword.Login, changePassword.Password);

            return RedirectToAction(nameof(Detail), new { _usersRepository.TryGetByLogin(changePassword.Login)?.Id });
        }


        public IActionResult ChangeRole(Guid id)
        {
            var existingUser = _usersRepository.TryGetById(id);

            var changeRole = new ChangeRole()
            {
                Login = existingUser?.Login,
                Role = existingUser?.Role?.ToString(),
                Roles = _rolesRepository.GetAll().Select(role => new SelectListItem() { Value = role.Name.ToString(), Text = role.Name }).ToList()
            };


            return View(changeRole);
        }


        [HttpPost]
        public IActionResult ChangeRole(ChangeRole changeRole)
        {
            if (!ModelState.IsValid)
            {
                return View(changeRole);
            }

            _usersRepository.ChangeRole(changeRole.Login, _rolesRepository.TryGetByName(changeRole.Role));

            return RedirectToAction(nameof(Detail), new { _usersRepository.TryGetByLogin(changeRole.Login)?.Id });
        }
    }
}
