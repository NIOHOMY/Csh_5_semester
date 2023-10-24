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
    public class Book
    {
        public int BookId { get; set; }
        [Required(ErrorMessage = "Название книги обязательно.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Идентификатор первого автора обязателен.")]
        public int FirstAuthorId { get; set; }
        [ForeignKey("FirstAuthorId")]
        public Author FirstAuthor { get; set; }
        [Required(ErrorMessage = "Год публикации обязателен.")]
        public int YearOfPublication { get; set; }
        [Required(ErrorMessage = "Цена обязательна.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть положительным числом.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Количество экземпляров обязательно.")]
        [Range(1, int.MaxValue, ErrorMessage = "Количество экземпляров должно быть положительным числом.")]
        public int NumberOfExamples { get; set; }
        [Required(ErrorMessage = "Идентификатор издателя обязателен.")]
        public int PublisherId { get; set; }
        [ForeignKey("PublisherId")]
        public Publisher Publisher { get; set; }
        public List<Issue>? Issues { get; set; } = new();
    }
}
