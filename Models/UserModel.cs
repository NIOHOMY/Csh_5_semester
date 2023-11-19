using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class UserModel : IdentityUser
    {
        public int UserModelId { get; set; }
        [Required(ErrorMessage = "Почта читателя обязательна.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пароль читателя обязателен.")]
        public string PasswordHash { get; set; }
    }
}
