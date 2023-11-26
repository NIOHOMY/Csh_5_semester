using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Reader
    {
        public int ReaderId { get; set; }

        [Required(ErrorMessage = "Почта читателя обязательна.")]
        [Display(Name = "Почта/Логин")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Имя читателя обязательно.")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Фамилия читателя обязательна.")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; set; } = "";

        [Required(ErrorMessage = "Адрес обязателен.")]
        [Display(Name = "Адрес проживания")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Номер телефона обязателен.")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Номер телефона")]
        public string FormattedPhoneNumber
        {
            get
            {
                if (string.IsNullOrEmpty(PhoneNumber))
                {
                    return string.Empty;
                }

                var numericPhone = new string(PhoneNumber.Where(char.IsDigit).ToArray());

                if (numericPhone.Length != 11)
                {
                    return PhoneNumber; 
                }

                if (numericPhone.Substring(0, 1) == "8")
                    numericPhone = "7" + numericPhone.Substring(1);

                return $"+{numericPhone.Substring(0, 1)} ({numericPhone.Substring(1, 3)}) {numericPhone.Substring(4, 3)}-{numericPhone.Substring(7, 2)}-{numericPhone.Substring(9, 2)}";
            }
        }
        public void SetFullName(string fullName)
        {
            var names = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (names.Length == 3)
            {
                FirstName = names[0];
                LastName = names[1];
                Patronymic = names[2];
            }
            else if (names.Length == 2)
            {
                FirstName = names[0];
                LastName = names[1];
                Patronymic = "";
            }
            else
            {
                throw new ArgumentException("Неверный формат ФИО. Пожалуйста, используйте формат \"Имя Фамилия Отчество\" или \"Имя Фамилия\".");
            }
        }
    }

}
