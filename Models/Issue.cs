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
    public class Issue
    {
        public int IssueId { get; set; }
        [Required(ErrorMessage = "Дата выдачи обязательна.")]
        public DateTime IssueDate { get; set; }
        [Required(ErrorMessage = "Дата возврата обязательна.")]
        public DateTime ReturnDate { get; set; }
        [Required(ErrorMessage = "Идентификатор читателя обязателен.")]
        public int ReaderId { get; set; }
        [ForeignKey("ReaderId")]
        public Reader Reader { get; set; }
        public List<Book> Books { get; set; } = new();
    }
}
