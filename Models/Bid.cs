using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Models
{
    public class Bid
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string UserId { get; set; }
        public IEnumerable<Car> Cars { get; set; }
        public double BidAmount { get; set; }
        public DateTime BidDateTime { get; set; }
    }
}
