using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication15.Models
{
    public class AllBooks
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public Nullable<int> Edition { get; set; }
        public Nullable<int> Price { get; set; }
        public string Status { get; set; }
        public int ID { get; set; }
    }
}