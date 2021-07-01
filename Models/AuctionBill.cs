using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Models
{
    public class AuctionBill
    {
        public int Id { get; set; }
        public Car Car { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CarSoldDate { get; set; }

    }
}
