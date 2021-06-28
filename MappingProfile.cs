using AutoMapper;
using CarShop.Models;
using CarShop.ViewModels;
using CarShop.ViewModels.Bids;
using CarShop.ViewModels.CarsAndReviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Car, CarViewModel>().ReverseMap();
            CreateMap<Review, ReviewViewModel>().ReverseMap();
            CreateMap<Car, CarWithReviewViewModel>();
            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();
            CreateMap<Bid, BidForUserResponse>().ReverseMap();
        }
    }
}
