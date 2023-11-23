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
    public class Book
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Название книги обязательно.")]
        [Display(Name = "Название книи")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Идентификатор первого автора обязателен.")]
        public int FirstAuthorId { get; set; }

        [Required(ErrorMessage = "Идентификатор первого автора обязателен.")]
        [ForeignKey("FirstAuthorId")]
        [Display(Name = "Автор")]
        public Author FirstAuthor { get; set; }

        [Required(ErrorMessage = "Год публикации обязателен.")]
        [Display(Name = "Год публикации")]
        public int YearOfPublication { get; set; }

        [Required(ErrorMessage = "Цена обязательна.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть положительным числом.")]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Количество экземпляров обязательно.")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество экземпляров должно быть неотрицательным числом.")]
        [Display(Name = "Количество экземпляров")]
        public int NumberOfExamples { get; set; }

        [Display(Name = "Обложка")]
        public byte[]? ImageData { get; set; }

        [Display(Name = "Описание")]
        public string? Info { get; set; }

        [Required(ErrorMessage = "Идентификатор издателя обязателен.")]
        public int PublisherId { get; set; }

        [ForeignKey("PublisherId")]
        [Display(Name = "Издательство")]
        public Publisher Publisher { get; set; }

        public List<Issue>? Issues { get; set; } = new();
    }
}
