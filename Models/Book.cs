using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int FirstAuthorId { get; set; }
        public int YearOfPublication { get; set; }
        public decimal Price { get; set; }
        public int NumberOfExamples { get; set; }
        public int PublisherId { get; set; }

        [ForeignKey("FirstAuthorId")]
        public Author FirstAuthor { get; set; }

        [ForeignKey("PublisherId")]
        public Publisher Publisher { get; set; }
    }
}
