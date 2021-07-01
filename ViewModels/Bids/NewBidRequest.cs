using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.ViewModels.Bids
{
    public class NewBidRequest
    {
        public int BidCarId { get; set; }
        public double BidAmount { get; set; }
        public DateTime BidDateTime { get; set; }
        public ApplicationUserViewModel ApplicationUser { get; set; }
    }
}
