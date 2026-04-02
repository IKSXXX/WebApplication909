using System.ComponentModel.DataAnnotations;
using WebApplication909.Helpers;

namespace WebApplication909.Models
{
    public class DeliveryUserViewModel
    {
        [Display(Name = "Имя покупателя", Prompt = "Ваше имя")]
        [Required(ErrorMessage = "Не указано имя покупателя")]
        [DataType(DataType.Text)]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Имя должно быть от {2} до {1} символов")]
        public required string Name { get; set; }


        [Display(Name = "Адрес доставки", Prompt = "Ваш адрес")]
        [Required(ErrorMessage = "Не указан адрес доставки")]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Адрес должен быть от {2} до {1} символов")]
        public required string Address { get; set; }


        [Display(Name = "Телефон", Prompt = "Ваш телефон")]
        [Required(ErrorMessage = "Не указан телефон покупателя")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Телефон может содержать только цифры")]
        [StringLength(16, MinimumLength = 5, ErrorMessage = "Телефон должен быть от {2} до {1} символов")]
        public required string Phone { get; set; }


        [Display(Name = "Дата доставки")]
        [Required(ErrorMessage = "Не указана дата доставки")]
        [DateRange]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }


        [Display(Name = "Комментарий", Prompt = "Ваш комментарий")]
        [MaxLength(512, ErrorMessage = "Максимальная длина комментария {1} символов")]
        [DataType(DataType.MultilineText)]
        public string? Comment { get; set; }

        [Required]
        public Guid Id { get; set; }
    }
}
