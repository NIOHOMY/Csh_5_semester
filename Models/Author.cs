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
    public class Author
    {
        public int AuthorId { get; set; }

        [Display(Name = "Фото")]
        public byte[]? ImageData { get; set; }

        [Display(Name = "Биография и другая информация")]
        public string? Info { get; set; }

        [Required(ErrorMessage = "Имя автора обязательно.")]
        [Display(Name = "ФИО")]
        public string Name { get; set; }
    }
}
