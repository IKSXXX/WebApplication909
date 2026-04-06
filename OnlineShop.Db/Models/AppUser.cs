using Microsoft.AspNetCore.Identity;

namespace OnlineShop.Db.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime CreationDateTime { get; set; } = DateTime.UtcNow;
    }
}