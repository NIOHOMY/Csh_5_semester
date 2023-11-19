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
    public class Issue
    {
        public int IssueId { get; set; }
        [Required(ErrorMessage = "Дата выдачи обязательна.")]
        public DateTime IssueDate { get; set; }
        [Required(ErrorMessage = "Дата возврата обязательна.")]
        public DateTime ReturnDate { get; set; }
        public bool isСonfirmed { get; set; } = false;
        [Required(ErrorMessage = "Идентификатор читателя обязателен.")]
        public int ReaderId { get; set; }
        [Required(ErrorMessage = "Стоимость обязательна.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Стоимость должна быть положительным числом.")]
        public decimal Price { get; set; } = 0;
        [ForeignKey("ReaderId")]
        public Reader Reader { get; set; }
        public List<Book> Books { get; set; } = new();
    }
}
