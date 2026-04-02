using System.ComponentModel.DataAnnotations;

namespace WebApplication909.Models
{
    public class RegistrationViewModel
    {
        [Display(Name = "Имя пользователя", Prompt = "Имя пользователя")]
        [Required(ErrorMessage = "Не указано имя пользователя")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "От 3 до 50 символов")]
        public required string UserName { get; set; }

        [Display(Name = "Email", Prompt = "Ваш email")]
        [Required(ErrorMessage = "Не указан email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Введите валидный email")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email должен быть от {2} до {1} символов")]
        public required string Email { get; set; }   // обязательно!

        [Display(Name = "Пароль", Prompt = "Ваш пароль")]
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть от {2} до {1} символов")]
        public required string Password { get; set; }

        [Display(Name = "Подтвердите пароль", Prompt = "Подтвердите пароль")]
        [Required(ErrorMessage = "Не указан повторный пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public required string ConfirmPassword { get; set; }

        [Display(Name = "Телефон", Prompt = "Ваш телефон")]
        [Required(ErrorMessage = "Не указан телефон")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Введите корректный номер телефона")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Телефон должен быть от {2} до {1} символов")]
        public required string Phone { get; set; }

        [Display(Name = "Имя", Prompt = "Ваше имя")]
        [Required(ErrorMessage = "Не указано имя")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя должно быть от {2} до {1} символов")]
        public required string FirstName { get; set; }

        [Display(Name = "Фамилия", Prompt = "Ваша фамилия")]
        [Required(ErrorMessage = "Не указана фамилия")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Фамилия должна быть от {2} до {1} символов")]
        public required string LastName { get; set; }
    }
}