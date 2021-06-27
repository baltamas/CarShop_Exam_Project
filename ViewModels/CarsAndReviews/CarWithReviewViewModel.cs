﻿using CarShop.ViewModels.CarsAndReviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarBid.ViewModels.Cars
{
    public class CarWithReviewViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int MileAge { get; set; }
        public int Year { get; set; }
        public IEnumerable<ReviewViewModel> Reviews { get; set; }
    }
}