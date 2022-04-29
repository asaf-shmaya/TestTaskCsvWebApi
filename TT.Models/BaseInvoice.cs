﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Models
{
    public class BaseInvoice
    {
        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a valid invoice number")]
        public int Number { get; set; }
        
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid double number.")]
        public double Amount { get; set; }        
    }
}
