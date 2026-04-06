using System.ComponentModel.DataAnnotations;

namespace WebApplication909.Areas.Admin.Models
{
    public class AddUserViewModel
    {
        [Required(ErrorMessage = "Укажите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Укажите имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Укажите фамилию")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Укажите телефон")]
        [Phone]
        public string Phone { get; set; }
    }
}