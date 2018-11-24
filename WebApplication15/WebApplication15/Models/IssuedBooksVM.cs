using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication15.Models
{
    public class IssuedBooksVM
    {
        public string Number { get; set; }
        public string Email { get; set; }
        public DateTime Issue_date { get; set; }
        public DateTime Return_date { get; set; }
        public DateTime Due_date { get; set; }
        public int Fine { get; set; }
        public string Status { get; set; }
        public int ID { get; set; }
    }
}