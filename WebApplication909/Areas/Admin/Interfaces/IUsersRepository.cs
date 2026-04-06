using WebApplication909.Areas.Admin.Models;
using WebApplication909.Models;

namespace WebApplication909.Areas.Admin.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> TryGetByIdAsync(Guid userId);
        Task<User?> TryGetByLoginAsync(string login);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid userId);
        Task ChangePasswordAsync(string login, string newPassword);
        Task ChangeRoleAsync(string login, Role? newRole);
    }
}