using CarShop.ErrorHandling;
using CarShop.Models;
using CarShop.ViewModels.Bids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Services.BidService
{
    public interface IBidManagementService 
    {
        Task<ServiceResponse<Bid, IEnumerable<EntityError>>> PlaceBid (NewBidRequest newBidRequest, ApplicationUser user);

        Task<ServiceResponse<BidForUserResponse, IEnumerable<EntityError>>> GetAll(ApplicationUser user);

        Task<ServiceResponse<Bid, IEnumerable<EntityError>>> UpdateBid(int id, NewBidRequest updateBidRequest, ApplicationUser user);

        Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteBid(int id);

        bool BidExists(int id);
    }
}
