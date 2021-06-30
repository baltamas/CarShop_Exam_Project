using CarShop.Data;
using CarShop.ViewModels.CarsAndReviews;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Validators
{
    public class ReviewValidator : AbstractValidator<ReviewViewModel>
    {
        private readonly ApplicationDbContext _context;
        public ReviewValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(r => r.Content).NotEmpty().Length(5, 50).WithMessage("Please enter your review!");
            RuleFor(r => r.DateTime).NotNull().WithMessage("Date & time is a must! Please complete it!");
        }
    }
}
