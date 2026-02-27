using System.ComponentModel.DataAnnotations;

namespace WebApplication909.Areas.Admin.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public string? Email { get; set; }
    }
}
