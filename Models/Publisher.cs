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
    public class Publisher
    {
        public int PublisherId { get; set; }

        [Required(ErrorMessage = "Название издателя обязательно.")]
        [Display(Name = "Название")]
        public string NameOfPublisher { get; set; }

        [Required(ErrorMessage = "Город обязателен.")]
        [Display(Name = "Город")]
        public string City { get; set; }
    }
}
