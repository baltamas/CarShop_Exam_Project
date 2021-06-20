using AutoMapper;
using CarShop.Models;
using CarShop.ViewModels;
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
        }
    }
}
