using Microsoft.AspNetCore.Mvc;
using WebApplication909.Areas.Admin.Interfaces;
using WebApplication909.Models;

namespace WebApplication909.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController(IUsersRepository usersRepository) : Controller
    {
        IUsersRepository _usersRepository = usersRepository;
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
    }
}
