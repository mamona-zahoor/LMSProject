using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication15.Models
{
    public class IssuedBooksVM
    {
        [Required(ErrorMessage = "Please Enter Book Number")]
        public string Number { get; set; }
        public int UserID { get; set; }
        [Required(ErrorMessage = "Please Enter your Email Address")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter the date")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime Issue_date { get; set; }
        [Required(ErrorMessage = "Please Enter the date")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public Nullable<System.DateTime> Return_date { get; set; }
        [Required(ErrorMessage = "Please Enter the date")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime Due_date { get; set; }
       
        public int Fine { get; set; }
        public string Status { get; set; }
        public int ID { get; set; }
    }
}