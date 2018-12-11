using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication15.Models
{
    public class Teacher
    {
        [Required(ErrorMessage ="Please Enter your name")]
        [RegularExpression(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "Enter a valid Name")]
        public string Name { get; set; }


        [EmailAddress(ErrorMessage = "Please Enter your Email Address")]
        [Required(ErrorMessage = "Please Enter your Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter your Designation")]
       
       [RegularExpression(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "Enter a valid Designation")]

        public string Designation { get; set; }
        public int ID { get; set; }
        public string ResetPassword { get; set; }

    }
}