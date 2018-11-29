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
        public string Name { get; set; }


        [Required(ErrorMessage = "Please Enter your Email Address")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter your Designation")]
        [StringLength(20,MinimumLength =10)]
       
        public string Designation { get; set; }
        public int ID { get; set; }
        public string ResetPassword { get; set; }

    }
}