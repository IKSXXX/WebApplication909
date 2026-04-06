using System.ComponentModel.DataAnnotations;

namespace WebApplication909.Areas.Admin.Models
{
    public class ChangePasswordViewModel
    {
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите новый пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтвердите пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}