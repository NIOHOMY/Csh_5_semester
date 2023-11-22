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
        public string Email { get; set; }

        [Required(ErrorMessage = "Имя читателя обязательно.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Фамилия читателя обязательна.")]
        public string LastName { get; set; }

        public string Patronymic { get; set; } = "";

        [Required(ErrorMessage = "Адрес обязателен.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Номер телефона обязателен.")]
        public string PhoneNumber { get; set; }

        public string FormattedPhoneNumber
        {
            get
            {
                if (string.IsNullOrEmpty(PhoneNumber))
                {
                    return string.Empty;
                }

                // Преобразование в числовой формат
                var numericPhone = new string(PhoneNumber.Where(char.IsDigit).ToArray());

                // Проверка на длину
                if (numericPhone.Length != 11)
                {
                    return PhoneNumber; // Если не удалось распознать, вернуть исходный номер
                }

                // Форматирование по заданному шаблону
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
