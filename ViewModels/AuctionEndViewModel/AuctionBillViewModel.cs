using CarShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.ViewModels.AuctionEndViewModel
{
    public class AuctionBillViewModel
    {
        public AuctionBillViewModel(AuctionBill auctionBill)
        {
            Id = auctionBill.Id;
            Car = auctionBill.Car;
            User = auctionBill.User;
            CarSoldDate = auctionBill.CarSoldDate;
        }
        public int Id { get; set; }
        public Car Car { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CarSoldDate { get; set; }

    }
}
