using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerBankApplication.Models
{
    public class ResgistrationPageVM
    {
        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Balance required")]
        [Range(500, Double.MaxValue, ErrorMessage = "Balance should be greater than 500")]
        public double Balance { get; set; }
    }
}