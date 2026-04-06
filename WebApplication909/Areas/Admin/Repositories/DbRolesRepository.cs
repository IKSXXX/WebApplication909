using Microsoft.AspNetCore.Identity;
using WebApplication909.Areas.Admin.Interfaces;
using WebApplication909.Areas.Admin.Models;

namespace WebApplication909.Areas.Admin.Repositories
{
    public class DbRolesRepository : IRolesRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbRolesRepository(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<Role>> GetAllAsync()
        {
            var roles = _roleManager.Roles.ToList();
            return roles.Select(r => new Role
            {
                Id = Guid.Parse(r.Id),
                Name = r.Name ?? ""
            }).ToList();
        }

        public async Task<Role?> TryGetByNameAsync(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);
            if (role == null) return null;
            return new Role
            {
                Id = Guid.Parse(role.Id),
                Name = role.Name ?? ""
            };
        }

        public async Task AddAsync(Role role)
        {
            if (!await _roleManager.RoleExistsAsync(role.Name))
                await _roleManager.CreateAsync(new IdentityRole(role.Name));
        }
    }
}