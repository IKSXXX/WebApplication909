using WebApplication909.Areas.Admin.Models;

namespace WebApplication909.Areas.Admin.Interfaces
{
    public interface IRolesRepository
    {
        Task<List<Role>> GetAllAsync();
        Task<Role?> TryGetByNameAsync(string name);
        Task AddAsync(Role role);
    }
}