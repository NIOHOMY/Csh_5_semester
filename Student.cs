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
        public int StudentId { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }
        public string? Age { get; set; }
        public int Birthday { get; set; } = 0;
        [Required]
        public int GroupId { get; set; }
        public Group? Group { get; set; }

    }
}
