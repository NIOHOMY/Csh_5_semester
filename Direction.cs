using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Direction
    {
        public int DirectionId { get; set; }
        [Required]
        [MaxLength(50)]
        public string? DirectionName { get; set; }

        public int? GroupId { get; set; }
        public Group? Group { get; set; }
    }
}
