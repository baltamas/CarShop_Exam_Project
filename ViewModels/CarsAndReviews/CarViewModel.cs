using CarShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.ViewModels.CarsAndReviews
{
    public class CarViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int MileAge { get; set; }
        public int Year { get; set; }
        public CarFuelType CarFuelType { get; set; }
        public double StartingBid { get; set; }
        public DateTime BidStart { get; set; }
        public DateTime BidEnd { get; set; }
        public bool CarSold { get; set; }
        public string Color { get; set; }
        public string Engine { get; set; }
    }
}
