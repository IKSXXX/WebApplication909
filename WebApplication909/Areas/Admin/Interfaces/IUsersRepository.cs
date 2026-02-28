using WebApplication909.Areas.Admin.Models;
using WebApplication909.Models;

namespace WebApplication909.Areas.Admin.Interfaces
{
    public interface IUsersRepository
    {
        void Add(User user);
        User? TryGetByLogin(string login);
        List<User> GetAll();
        User? TryGetById(Guid userId);
        void Delete(Guid userId);
        void Update(User user);
        void ChangePassword(string login, string newPassword);
        void ChangeRole(string login, Role? newRole);
    }
}
