using Microsoft.AspNetCore.Identity;
using OnlineShop.Db.Models;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using WebApplication909.Models;

namespace OnlineShop.Db
{
    public static class IdentityInitializer
    {
        public static void Initialize(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@example.com";
            string adminPassword = "Admin123!";

            if (roleManager.FindByNameAsync(Constants.AdminRoleName).Result == null)
            {
                roleManager.CreateAsync(new IdentityRole(Constants.AdminRoleName)).Wait();
            }
            if (roleManager.FindByNameAsync(Constants.UserRoleName).Result == null)
            {
                roleManager.CreateAsync(new IdentityRole(Constants.UserRoleName)).Wait();
            }
            if (userManager.FindByNameAsync(adminEmail).Result == null)
            {
                var admin = new User { Email = adminEmail, UserName = adminEmail };
                var result = userManager.CreateAsync(admin, adminPassword).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, Constants.AdminRoleName).Wait();
                }
            }
        }
    }
}