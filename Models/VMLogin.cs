using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class VMLogin
    {
        [Required(ErrorMessage = "Почта читателя обязательна.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен.")]
        public string? PassWord { get; set; }

        public bool KeepLoggedIn { get; set; }
    }
}
