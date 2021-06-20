using CarShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.ViewModels
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
    }
}
