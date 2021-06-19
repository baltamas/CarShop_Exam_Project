using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }
        public Car Car { get; set; }
    }
}
