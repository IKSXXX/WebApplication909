using System.ComponentModel.DataAnnotations;

namespace WebApplication909.Models
{
    public class AuthorizationViewModel
    {
        [EmailAddress(ErrorMessage = "Введите валидную email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Логин должен быть от 5 до 30 символов")]
        [Display(Name = "Логин", Prompt = "qwerty@mail.ru")]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 50 символов")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        [Required]
        public bool IsRememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }

}
