using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class VMRegistration
    {
        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Почта/Логин")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Имя читателя обязательно.")]
        [Display(Name = "Имя")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Фамилия читателя обязательна.")]
        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; set; } = "";

        [Required(ErrorMessage = "Адрес обязателен.")]
        [Display(Name = "Адрес проживания")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Номер телефона обязателен.")]
        [Display(Name = "Номер телефона")]
        public string? PhoneNumber { get; set; }
    }
}
