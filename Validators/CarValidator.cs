using CarShop.Data;
using CarShop.ViewModels.CarsAndReviews;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CarShop.Validators
{
    public class CarValidator : AbstractValidator<CarViewModel>
    {
        private readonly ApplicationDbContext _context;

        public CarValidator(ApplicationDbContext context)
        {
            _context = context;
            RuleFor(c => c.Name).NotEmpty().WithMessage("You need to introduce the car's name!")
                .Length(3, 25);
            RuleFor(c => c.Price).InclusiveBetween(1, 18700000).WithMessage("You need to introduce the car's price");
            RuleFor(c => c.Description).NotEmpty().Length(10, 100).WithMessage("You need to enter a short description about the car!");
            RuleFor(c => c.MileAge).InclusiveBetween(0, 1000000).WithMessage("Please enter the car's mileage!");
            RuleFor(c => c.Year).InclusiveBetween(1888, 2021).WithMessage("You need to specify the car's production year!");
            RuleFor(c => c.CarFuelType).NotNull();
            RuleFor(c => c.BidStart)
            .NotEmpty()
            .WithMessage("Auction start date is Required! ");
            RuleFor(c => c.BidEnd)
                .NotEmpty().WithMessage("Auction end date is required! ")
                .GreaterThan(c => c.BidStart.Date).WithMessage("Auction end date must be after start date");
            RuleFor(c => c.Color).NotEmpty().WithMessage("You need to introduce the car's color!");
            RuleFor(c => c.Engine).NotEmpty().WithMessage("You need to specify the car's engine! ");


        }
    }
}
