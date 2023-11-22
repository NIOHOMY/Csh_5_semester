﻿using System;
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
        [Required(ErrorMessage = "Имя автора обязательно.")]
        public string Name { get; set; }

        public byte[]? ImageData { get; set; }
        public string? Info { get; set; }
    }
}
