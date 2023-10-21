using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Имя является обязательным полем!")]
        [MaxLength(50)]
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "Фамилия является обязательным полем!")]
        [MaxLength(50)]
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "Отчество является обязательным полем!")]
        [MaxLength(50)]
        public string? Surname { get; set; }

        public string? Age { get; set; }

        public int Birthday { get; set; } = 0;

        [Required(ErrorMessage = "ID группы является обязательным полем!")]
        public int GroupId { get; set; }

        public Group? Group { get; set; }
    }
}
