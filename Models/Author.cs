using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
    }
}
