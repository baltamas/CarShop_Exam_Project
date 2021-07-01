using CarShop.ViewModels.CarsAndReviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.ViewModels.Bids
{
    public class BidForUserResponse
    {
        public int Id { get; set; }
        public CarViewModel Car { get; set; }
        public double BidAmount { get; set; }
        public DateTime BidDateTime { get; set; }
    }
}
