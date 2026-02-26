using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using WebApplication909.Models;

namespace WebApplication909.Controllers
{
    public class AccountController : Controller
    {
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
                    "Логин и пароль не должны совпадать");
            };
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

            if (!ModelState.IsValid)
            {
                return View(registration);
            }

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
