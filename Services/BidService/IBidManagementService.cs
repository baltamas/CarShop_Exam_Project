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
		Task<ServiceResponse<List<Bid>, IEnumerable<EntityError>>> GetBids(string userId);
		Task<ServiceResponse<Bid, IEnumerable<EntityError>>> GetBid(string userId, int id);
		Task<ServiceResponse<Bid, IEnumerable<EntityError>>> CreateBids(string userId, NewBidRequest newBidRequest);
		bool BidsExists(string userId, int favouritesId);
        Task<ServiceResponse<double, IEnumerable<EntityError>>> GetBidForCar(int carId);
    }
}
