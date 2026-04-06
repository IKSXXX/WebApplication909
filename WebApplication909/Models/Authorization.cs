using System.ComponentModel.DataAnnotations;

namespace WebApplication909.Models
{
    public class Authorization
    {
        [Required(ErrorMessage = "Введите логин")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool IsRememberMe { get; set; }
    }
}