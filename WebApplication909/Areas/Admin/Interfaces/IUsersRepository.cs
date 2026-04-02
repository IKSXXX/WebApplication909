using WebApplication909.Areas.Admin.Models;
using WebApplication909.Models;

namespace WebApplication909.Areas.Admin.Interfaces
{
    public interface IUsersRepository
    {
        void Add(UserViewModel user);
        UserViewModel? TryGetByLogin(string login);
        List<UserViewModel> GetAll();
        UserViewModel? TryGetById(Guid userId);
        void Delete(Guid userId);
        void Update(UserViewModel user);
        void ChangePassword(string login, string newPassword);
        void ChangeRole(string login, Role? newRole);
    }
}
