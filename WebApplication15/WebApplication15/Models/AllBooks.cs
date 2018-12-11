using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication15.Models
{
    public class AllBooks
    {
        [Required(ErrorMessage = "Please Enter Book Number")]
       // [RegularExpression(@"[^A-Za-z0-9]+", ErrorMessage = "Enter a valid Book Number")]
         public string Number { get; set; }


        [Required(ErrorMessage = "Please Enter Book Name")]
        [RegularExpression(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "Enter a valid Name")]

        public string Name { get; set; }

        [RegularExpression(@"^[0-9]*$",ErrorMessage ="Enter a valid price")]
        [Required(ErrorMessage = "Please Enter Price")]
        public int Price { get; set; }


        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Enter a valid Edition")]
        [Required(ErrorMessage = "Please Enter Edition")]
        public int Edition { get; set; }


        [Required(ErrorMessage = "Please Enter author name")]
        
        [RegularExpression(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "Enter a valid Author Name")]

        public string Author { get; set; }
       
        public string ImagePath { get; set; }
        public string Status { get; set; }
        public int ID { get; set; }
    }
}