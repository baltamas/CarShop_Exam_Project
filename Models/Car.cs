using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Models
{
    public enum CarFuelType { Gasoline, Diesel, BioDiesel, Ethanol, Other };
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int MileAge { get; set; }
        public int Year { get; set; }
        public CarFuelType CarFuelType { get; set; }
        public List<Review> Reviews { get; set; }

    }
}
