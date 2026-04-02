using System.ComponentModel.DataAnnotations;
using WebApplication909.Areas.Admin.Models;

namespace WebApplication909.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Имя пользователя")]
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "От 3 до 50 символов")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Введите валидный email")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "От 5 до 100 символов")]
        public string Email { get; set; }

        [Display(Name = "Пароль (только для создания)")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 символов")]
        public string Password { get; set; }

        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Телефон")]
        [Phone(ErrorMessage = "Некорректный телефон")]
        [StringLength(20, ErrorMessage = "Не более 20 символов")]
        public string Phone { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "От 2 до 50 символов")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Фамилия обязательна")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "От 2 до 50 символов")]
        public string LastName { get; set; }

        [Display(Name = "Роль")]
        public string RoleName { get; set; }

        [Display(Name = "Дата регистрации")]
        public DateTime CreationDateTime { get; set; }
    }
}
