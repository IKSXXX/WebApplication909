using WebApplication909.Areas.Admin.Models;

namespace WebApplication909.Areas.Admin.Interfaces
{
    public interface IRolesRepository
    {
        List<Role> GetAll();

        Role? TryGetByName(string roleName);

        Role? TryGetById(Guid roleId);

        void Add(Role role);

        void Delete(Guid roleId);

    }
}
