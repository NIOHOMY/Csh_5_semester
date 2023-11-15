using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class VMRegistration
    {
        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Имя читателя обязательно.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Фамилия читателя обязательна.")]
        public string? LastName { get; set; }

        public string Patronymic { get; set; } = "";

        [Required(ErrorMessage = "Адрес обязателен.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Номер телефона обязателен.")]
        public string? PhoneNumber { get; set; }
    }
}
