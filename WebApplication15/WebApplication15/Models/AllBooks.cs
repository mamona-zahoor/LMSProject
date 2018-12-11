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
        public string Number { get; set; }
        [Required(ErrorMessage = "Please Enter Book Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter Price")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Please Enter Edition")]
        public int Edition { get; set; }
        [Required(ErrorMessage = "Please Enter author name")]
        public string Author { get; set; }
       
       
        [Required(ErrorMessage = "Please Select a status")]
        public string Status { get; set; }
        public int ID { get; set; }
    }
}