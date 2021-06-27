using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.ViewModels.Bids
{
    public class UpdateBidForUser
    {
        public int Id { get; set; }
        public ApplicationUserViewModel User { get; set; }
        public List<int> CarIds { get; set; }
        public double BidAmount { get; set; }
        public DateTime BidDateTime { get; set; }
    }
}
