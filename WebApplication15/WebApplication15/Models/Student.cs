using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication15.Models
{
    public class Student
    {
        [Required]
         [RegularExpression(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage ="Enter a valid Name")]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "Please Enter your Email Address")]
        [Required(ErrorMessage = "Please Enter your Email Address")]
        
        public string Email { get; set; }

       
        [Display(Name = "Registration Number")]
        // [StringLength(11, ErrorMessage = "Enter a Valid Registration Number", MinimumLength = 9)]
        [RegularExpression(@"^[0-9]{4}[-][a-zA-Z]{2,3}[-][0-9]{2,3}$", ErrorMessage = "Please Enter a valid formatt for Registration Number")]

        public string Registration_Number { get; set; }
       
        public int ID { get; set; }
        public string ResetPassword { get; set; }
    }
}