﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Group
    {
        public int GroupId { get; set; }
        [Required(ErrorMessage = " ! Обязательное поле название группы")]
        [MaxLength(50)]
        public string? GroupName { get; set; }

        public List<Student> Students { get; set; } = new();
    }
}