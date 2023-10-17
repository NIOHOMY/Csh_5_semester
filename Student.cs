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
        [MaxLength(30)]
        public string? Name { get; set; }
        public string? Age { get; set; }
        public int Birthday { get; set; } = 0;

        public int GroupId { get; set; }
        public Group? Group { get; set; }

    }
}
