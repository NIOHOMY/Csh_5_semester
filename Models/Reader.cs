using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class Reader
    {
        public int ReaderId { get; set; }
        [Required(ErrorMessage = "Имя читателя обязательно.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Фамилия читателя обязательна.")]
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        [Required(ErrorMessage = "Адрес обязателен.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Номер телефона обязателен.")]
        public string PhoneNumber { get; set; }

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
                Patronymic = null;
            }
            else
            {
                throw new ArgumentException("Неверный формат ФИО. Пожалуйста, используйте формат \"Имя Фамилия Отчество\" или \"Имя Фамилия\".");
            }
        }
    }

}
