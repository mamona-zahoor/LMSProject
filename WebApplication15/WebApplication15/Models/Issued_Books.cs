//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication15.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class Issued_Books
    {
        public string Number { get; set; }
        public string Email { get; set; }
        [Required(AllowEmptyStrings =true)]
        public DateTime Return_date { get; set; }
        public System.DateTime Issue_date { get; set; }
        public System.DateTime Due_date { get; set; }
        public int Fine { get; set; }
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Status { get; set; }
    
        public virtual User User { get; set; }
    }
}
