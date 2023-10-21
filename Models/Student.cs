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
        [Required(ErrorMessage = " ! Обязательное поле имя")]
        [MaxLength(50)]
        public string? Firstname { get; set; }
        [Required(ErrorMessage = " ! Обязательное поле фамилия")]
        [MaxLength(50)]
        public string? Lastname { get; set; }
        [Required(ErrorMessage = " ! Обязательное поле отчество")]
        [MaxLength(50)]
        public string? Surname { get; set; }
        public string? Age { get; set; }
        public int Birthday { get; set; } = 0;
        [Required(ErrorMessage = " ! Обязательное поле id группы")]
        public int GroupId { get; set; }
        public Group? Group { get; set; }

    }
}
