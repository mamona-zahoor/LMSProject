using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication15.Models
{
    public class Student
    {
        [Required(ErrorMessage = "Please Enter your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter your Email Address")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Registration Number")]
        [StringLength(11, ErrorMessage = "Registration Number is not valid", MinimumLength = 9)]
        [DisplayFormat(DataFormatString = "2014-CS-001")]
        public string Registration_Number { get; set; }
        public int ID { get; set; }
        public string ResetPassword { get; set; }
    }
}