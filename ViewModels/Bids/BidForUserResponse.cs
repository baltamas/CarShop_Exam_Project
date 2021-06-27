﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.ViewModels.Bids
{
    public class BidForUserResponse
    {
        public ApplicationUserViewModel ApplicationUser { get; set; }
        public List<CarViewModel> Cars { get; set; }
        public double BidAmount { get; set; }
        public DateTime BidDateTime { get; set; }
    }
}
