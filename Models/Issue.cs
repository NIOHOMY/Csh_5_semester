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
        [Display(Name = "Номер выдачи")]
        public int IssueId { get; set; }

        [Required(ErrorMessage = "Дата выдачи обязательна.")]
        [Display(Name = "Дата выдачи")]
        public DateTime IssueDate { get; set; }

        [Required(ErrorMessage = "Дата возврата обязательна.")]
        [Display(Name = "Дата возврата")]
        public DateTime ReturnDate { get; set; }

        [Display(Name = "Статус")]
        public bool isСonfirmed { get; set; } = false;

        [Required(ErrorMessage = "Идентификатор читателя обязателен.")]
        public int ReaderId { get; set; }

        [Required(ErrorMessage = "Стоимость обязательна.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Стоимость должна быть положительным числом.")]
        [Display(Name = "Итоговая стоимость")]
        public decimal Price { get; set; } = 0;

        [ForeignKey("ReaderId")]
        [Display(Name = "Читатель")]
        public Reader Reader { get; set; }

        [Display(Name = "Список книг")]
        public List<Book> Books { get; set; } = new();
    }
}
