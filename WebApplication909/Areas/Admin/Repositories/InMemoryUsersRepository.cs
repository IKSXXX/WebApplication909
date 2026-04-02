using Microsoft.AspNetCore.Identity;
using WebApplication909.Areas.Admin.Interfaces;
using WebApplication909.Areas.Admin.Models;
using WebApplication909.Models;

namespace WebApplication909.Areas.Admin.Repositories
{
    public class InMemoryUsersRepository : IUsersRepository
    {
        private readonly List<User> _users = [];

        public InMemoryUsersRepository(IRolesRepository rolesRepository)
        {
            // Добавляем администратора, если нет пользователей
            if (!_users.Any())
            {
                var adminRole = rolesRepository.TryGetByName("Admin");
                var hasher = new PasswordHasher<User>();
                var admin = new User
                {
                    Id = Guid.NewGuid(),
                    Login = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "Adminov",
                    Phone = "+70000000000",
                    CreationDateTime = DateTime.Now,
                    Role = adminRole
                };
                admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");
                _users.Add(admin);
            }
        }

        public void Add(User user)
        {
            user.Id = Guid.NewGuid();
            user.CreationDateTime = DateTime.Now;
            _users.Add(user);
        }

        public User? TryGetByLogin(string login) =>
            _users.FirstOrDefault(user => user.Login == login);

        public List<User> GetAll() => _users;

        public User? TryGetById(Guid userId) =>
            _users.FirstOrDefault(user => user.Id == userId);

        public void Delete(Guid userId)
        {
            var existingUser = TryGetById(userId);
            if (existingUser != null) _users.Remove(existingUser);
        }

        public void Update(User user)
        {
            var existingUser = TryGetById(user.Id);
            if (existingUser != null)
            {
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Phone = user.Phone;
            }
        }

        public void ChangePassword(string login, string newPassword)
        {
            var user = TryGetByLogin(login);
            if (user != null)
            {
                var hasher = new PasswordHasher<User>();
                user.PasswordHash = hasher.HashPassword(user, newPassword);
            }
        }

        public void ChangeRole(string login, Role? newRole)
        {
            var user = TryGetByLogin(login);
            if (user != null) user.Role = newRole;
        }
    }
}