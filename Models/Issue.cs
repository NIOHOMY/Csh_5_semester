using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Models
{
    public class Issue
    {
        public int IssueId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int ReaderId { get; set; }

        [ForeignKey("ReaderId")]
        public Reader Reader { get; set; }
    }
}
