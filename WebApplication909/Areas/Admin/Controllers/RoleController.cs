using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;
using WebApplication909.Areas.Admin.Models;

namespace WebApplication909.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: /Admin/Role/Index
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var roleViewModels = roles.Select(r => new RoleViewModel
            {
                Id = r.Id,
                Name = r.Name ?? ""
            }).ToList();
            return View(roleViewModels);
        }

        // GET: /Admin/Role/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Admin/Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _roleManager.RoleExistsAsync(model.Name))
                {
                    ModelState.AddModelError("Name", "Роль с таким именем уже существует");
                    return View(model);
                }

                var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                if (result.Succeeded)
                {
                    TempData["Success"] = $"Роль '{model.Name}' успешно создана.";
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // GET: /Admin/Role/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            var model = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name ?? ""
            };
            return View(model);
        }

        // POST: /Admin/Role/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                if (role == null)
                    return NotFound();

                // Защита от переименования системной роли Admin
                if (role.Name == "Admin" && model.Name != "Admin")
                {
                    ModelState.AddModelError("", "Невозможно переименовать роль 'Admin'.");
                    return View(model);
                }

                if (await _roleManager.RoleExistsAsync(model.Name) && role.Name != model.Name)
                {
                    ModelState.AddModelError("Name", "Роль с таким именем уже существует");
                    return View(model);
                }

                role.Name = model.Name;
                role.NormalizedName = _roleManager.NormalizeKey(model.Name);
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    TempData["Success"] = $"Роль '{model.Name}' успешно обновлена.";
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }



        // POST: /Admin/Role/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                TempData["Error"] = "Роль не найдена.";
                return RedirectToAction(nameof(Index));
            }

            // Запрет удаления системных ролей Admin и User (опционально)
            if (role.Name == "Admin" || role.Name == "User")
            {
                TempData["Error"] = $"Невозможно удалить системную роль '{role.Name}'.";
                return RedirectToAction(nameof(Index));
            }

            // Проверка, есть ли пользователи с этой ролью
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
            if (usersInRole.Any())
            {
                TempData["Error"] = $"Невозможно удалить роль '{role.Name}', так как она назначена {usersInRole.Count} пользователю(ям). Сначала измените роли пользователей.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["Success"] = $"Роль '{role.Name}' успешно удалена.";
            }
            else
            {
                TempData["Error"] = "Ошибка при удалении роли.";
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}