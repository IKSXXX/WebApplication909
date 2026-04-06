using Microsoft.AspNetCore.Identity;
using OnlineShop.Db.Models;
using WebApplication909.Areas.Admin.Interfaces;
using WebApplication909.Areas.Admin.Models;
using WebApplication909.Models;

namespace WebApplication909.Areas.Admin.Repositories
{
    public class DbUsersRepository : IUsersRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbUsersRepository(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = _userManager.Users.ToList();
            var result = new List<User>();
            foreach (var appUser in users)
            {
                var roles = await _userManager.GetRolesAsync(appUser);
                var roleName = roles.FirstOrDefault() ?? "User";
                var role = await _roleManager.FindByNameAsync(roleName);
                var userModel = MapToUser(appUser, role);
                result.Add(userModel);
            }
            return result;
        }

        public async Task<User?> TryGetByIdAsync(Guid userId)
        {
            var appUser = await _userManager.FindByIdAsync(userId.ToString());
            if (appUser == null) return null;

            var roles = await _userManager.GetRolesAsync(appUser);
            var roleName = roles.FirstOrDefault() ?? "User";
            var role = await _roleManager.FindByNameAsync(roleName);
            return MapToUser(appUser, role);
        }

        public async Task<User?> TryGetByLoginAsync(string login)
        {
            var appUser = await _userManager.FindByNameAsync(login);
            if (appUser == null) return null;

            var roles = await _userManager.GetRolesAsync(appUser);
            var roleName = roles.FirstOrDefault() ?? "User";
            var role = await _roleManager.FindByNameAsync(roleName);
            return MapToUser(appUser, role);
        }

        public async Task AddAsync(User user)
        {
            var appUser = new AppUser
            {
                UserName = user.Login,
                Email = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                CreationDateTime = DateTime.UtcNow
            };
            var result = await _userManager.CreateAsync(appUser, user.PasswordHash); // Временно передаём пароль как строку
            if (result.Succeeded && user.Role != null)
            {
                await _userManager.AddToRoleAsync(appUser, user.Role.Name);
            }
        }

        public async Task UpdateAsync(User user)
        {
            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (appUser != null)
            {
                appUser.FirstName = user.FirstName;
                appUser.LastName = user.LastName;
                appUser.Phone = user.Phone;
                appUser.UserName = user.Login;
                appUser.Email = user.Login;
                await _userManager.UpdateAsync(appUser);
            }
        }

        public async Task DeleteAsync(Guid userId)
        {
            var appUser = await _userManager.FindByIdAsync(userId.ToString());
            if (appUser != null)
                await _userManager.DeleteAsync(appUser);
        }

        public async Task ChangePasswordAsync(string login, string newPassword)
        {
            var appUser = await _userManager.FindByNameAsync(login);
            if (appUser != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
                await _userManager.ResetPasswordAsync(appUser, token, newPassword);
            }
        }

        public async Task ChangeRoleAsync(string login, Role? newRole)
        {
            var appUser = await _userManager.FindByNameAsync(login);
            if (appUser != null && newRole != null)
            {
                var currentRoles = await _userManager.GetRolesAsync(appUser);
                await _userManager.RemoveFromRolesAsync(appUser, currentRoles);
                await _userManager.AddToRoleAsync(appUser, newRole.Name);
            }
        }

        // Вспомогательный метод преобразования AppUser -> User (ваша модель)
        private User MapToUser(AppUser appUser, IdentityRole? role)
        {
            return new User
            {
                Id = Guid.Parse(appUser.Id),
                Login = appUser.UserName ?? "",
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Phone = appUser.Phone,
                CreationDateTime = appUser.CreationDateTime,
                Role = role != null ? new Role { Id = Guid.Parse(role.Id), Name = role.Name } : null,
                PasswordHash = appUser.PasswordHash // можно не передавать
            };
        }
    }
}