using System.ComponentModel.DataAnnotations;

namespace WebApplication909.Areas.Admin.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите название роли")]
        [Display(Name = "Название роли")]
        public string Name { get; set; } = string.Empty;
    }
}