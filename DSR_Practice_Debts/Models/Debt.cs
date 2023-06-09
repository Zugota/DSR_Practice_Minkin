﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSR_Practice_Debts.Models
{
    public class Debt
    {
        [Key]
        public int IdDebt { get; set; }
        [RegularExpression("\b[0-9]+\b", ErrorMessage = "Введите верные данные")]
        public int Summ { get; set; }
        public DateTime Date { get; set; }

        public DateTime DateOfEnd { get; set; }

        public string? Status { get; set; }

        public int userId { get; set; }

        public User? User { get; set; }
         
        public DateTime? RealDateEnd { get; set; }
        
    }
}
