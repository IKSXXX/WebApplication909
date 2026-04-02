using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication909.Areas.Admin.Interfaces;
using WebApplication909.Areas.Admin.Models;
using WebApplication909.Models;
using WebApplication909.Helpers;

namespace WebApplication909.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users.ToUserViewModels());
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserViewModel user)
        {
            if (await _userManager.FindByNameAsync(user.UserName) != null)
            {
                ModelState.AddModelError("", "Такой пользователь уже существует!");
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var identityUser = new User
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.Phone,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            await _userManager.CreateAsync(identityUser, user.Password);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return View(user.ToUserViewModel());
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            return View(existingUser.ToUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.Phone;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;

                await _userManager.UpdateAsync(existingUser);
            }

            return RedirectToAction(nameof(Detail), new { id = user.Id });
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            var changePassword = new ChangePassword()
            {
                Login = existingUser?.UserName
            };

            return View(changePassword);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            if (changePassword.Login == changePassword.Password)
            {
                ModelState.AddModelError("", "Имя и пароль не должны совпадать");
            }

            if (!ModelState.IsValid)
            {
                return View(changePassword);
            }

            var user = await _userManager.FindByNameAsync(changePassword.Login);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, changePassword.Password);
            }

            return RedirectToAction(nameof(Detail), new { id = user?.Id });
        }

        public async Task<IActionResult> ChangeRole(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            var userRoles = await _userManager.GetRolesAsync(existingUser);

            var changeRole = new ChangeRole()
            {
                Login = existingUser?.UserName,
                Role = userRoles.FirstOrDefault(),
                Roles = _roleManager.Roles.Select(role => new SelectListItem()
                {
                    Value = role.Name,
                    Text = role.Name
                }).ToList()
            };

            return View(changeRole);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(ChangeRole changeRole)
        {
            if (!ModelState.IsValid)
            {
                return View(changeRole);
            }

            var user = await _userManager.FindByNameAsync(changeRole.Login);
            if (user != null && !string.IsNullOrEmpty(changeRole.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, changeRole.Role);
            }
            return RedirectToAction(nameof(Detail), new { id = user?.Id });
        }
    }
}
