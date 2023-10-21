using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Models
{
    public class Publisher
    {
        public int PublisherId { get; set; }
        public string NameOfPublisher { get; set; }
        public string City { get; set; }
    }
}
