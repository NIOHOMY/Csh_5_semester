using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class VMLogin
    {
        [Required(ErrorMessage = "Почта читателя обязательна.")]
        [Display(Name = "Почта/Логин")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен.")]
        [Display(Name = "Пароль")]
        public string? PassWord { get; set; }

        public bool KeepLoggedIn { get; set; }
    }
}
