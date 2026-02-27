using Microsoft.AspNetCore.Mvc;
using WebApplication909.Areas.Admin.Interfaces;
using WebApplication909.Areas.Admin.Models;

namespace WebApplication909.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly IRolesRepository _rolesRepository;


        public RoleController(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;

        }


        public IActionResult Index()
        {
            var roles = _rolesRepository.GetAll();

            return View(roles);
        }


        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Add(Role role)
        {
            if (_rolesRepository.TryGetByName(role.Name) != null)
            {
                ModelState.AddModelError("",
                    "Такая роль уже существует!");
            }

            if (!ModelState.IsValid)
            {
                return View(role);
            }

            _rolesRepository.Add(role);

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(Guid roleId)
        {
            _rolesRepository.Delete(roleId);

            return RedirectToAction(nameof(Index));
        }
    }
}
