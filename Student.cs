using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace ConsoleApp1
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [MaxLength(30)]
        public string? Name { get; set; }

        [Required]
        public string Age { get; set; }

        [Required]
        public int Birthday { get; set; } = 0;

        // связь с таблицей group, внешний ключ
        public int GroupId { get; set; }
        // навигационное свойство
        public Group? Group { get; set; }


        public List<Course> Courses { get; set; } = new();
    }
}
