using CarShop.Data;
using CarShop.ViewModels.Bids;
using CarShop.ViewModels.CarsAndReviews;
using FluentValidation;


namespace CarShop.Validators
{
    public class BidValidator : AbstractValidator<BidForUserResponse>
    {
        private readonly ApplicationDbContext _context;
        public BidValidator(ApplicationDbContext context)
        {
            
        }
    }
}
