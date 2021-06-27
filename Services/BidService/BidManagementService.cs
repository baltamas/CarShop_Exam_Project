using AutoMapper;
using CarShop.Data;
using CarShop.ErrorHandling;
using CarShop.Models;
using CarShop.ViewModels.Bids;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Services.BidService
{
    public class BidManagementService : IBidManagementService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public BidManagementService(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ServiceResponse<BidForUserResponse, IEnumerable<EntityError>>> GetAll(ApplicationUser user)
        {
            var bidFromDb = await _context.Bids
                .Where(b => b.ApplicationUser.Id == user.Id)
                .Include(b => b.Cars)
                .FirstOrDefaultAsync();

            var bidForUserResponse = _mapper.Map<BidForUserResponse>(bidFromDb);

            var serviceResponse = new ServiceResponse<BidForUserResponse, IEnumerable<EntityError>>();
            serviceResponse.ResponseOk = bidForUserResponse;

            return serviceResponse;
        }

        public async Task<ServiceResponse<Bid, IEnumerable<EntityError>>> PlaceBid(NewBidRequest newBidRequest, ApplicationUser user)
        {
            var serviceResponse = new ServiceResponse<Bid, IEnumerable<EntityError>>();

            var bidCars = new List<Car>();
            newBidRequest.ReservedCarIds.ForEach(bid =>
            {
                var carWithId = _context.Cars.Find(bid);
                if (carWithId != null)
                {
                    bidCars.Add(carWithId);
                }
            });

            var bid = new Bid
            {
                ApplicationUser = user,
                Cars = bidCars,
                BidAmount = newBidRequest.BidAmount,
                BidDateTime = newBidRequest.BidDateTime,
            };

            _context.Bids.Add(bid);

            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = bid;
            }
            catch (Exception e)
            {
                var errors = new List<EntityError>();
                errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Bid, IEnumerable<EntityError>>> UpdateBid(int id, NewBidRequest updateBidRequest, ApplicationUser user)
        {
            var serviceResponse = new ServiceResponse<Bid, IEnumerable<EntityError>>();

            var bidCars = new List<Car>();
            updateBidRequest.ReservedCarIds.ForEach(bid =>
            {
                var carWithId = _context.Cars.Find(bid);
                if (carWithId != null)
                {
                    bidCars.Add(carWithId);
                }
            });

            var bid = new Bid
            {
                Id = id,
                ApplicationUser = user,
                BidDateTime = updateBidRequest.BidDateTime,
                BidAmount = updateBidRequest.BidAmount,
                Cars = bidCars
            };

            _context.Entry(bid).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = bid;
            }
            catch (Exception e)
            {
                var errors = new List<EntityError>();
                errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteBid(int id)
        {
            var serviceResponse = new ServiceResponse<bool, IEnumerable<EntityError>>();

            try
            {
                var bid = await _context.Bids.FindAsync(id);
                _context.Bids.Remove(bid);
                await _context.SaveChangesAsync();
                serviceResponse.ResponseOk = true;
            }
            catch (Exception e)
            {
                var errors = new List<EntityError>();
                errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
                serviceResponse.ResponseError = errors;
            }

            return serviceResponse;
        }

        public bool BidExists(int id)
        {
            return _context.Bids.Any(e => e.Id == id);
        }
    }
}

