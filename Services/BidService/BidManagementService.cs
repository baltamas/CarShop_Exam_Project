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
		public ApplicationDbContext _context;
		public BidManagementService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<ServiceResponse<Bid, IEnumerable<EntityError>>> CreateBids(string userId, NewBidRequest newBidRequest)
		{
			var car = _context.Cars.Find(newBidRequest.BidCarId);
		
			var bid = new Bid
			{
				UserId = userId,
				Car = car,
				BidAmount = newBidRequest.BidAmount,
				BidDateTime = newBidRequest.BidDateTime
			};

			_context.Bids.Add(bid);
			var serviceResponse = new ServiceResponse<Bid, IEnumerable<EntityError>>();

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

		public async Task<ServiceResponse<List<Bid>, IEnumerable<EntityError>>> GetBids(string userId)
		{
			var result = await _context.Bids.Where(b => b.UserId == userId).Include(b => b.Car).OrderByDescending(c => c.BidAmount).ToListAsync();
			var serviceResponse = new ServiceResponse<List<Bid>, IEnumerable<EntityError>>();
			serviceResponse.ResponseOk = result;
			return serviceResponse;
		}

		public async Task<ServiceResponse<Bid, IEnumerable<EntityError>>> UpdateBid(Bid bids, UpdateBidForUser updateBidForUser)
		{
			_context.Entry(bids).State = EntityState.Modified;
			var serviceResponse = new ServiceResponse<Bid, IEnumerable<EntityError>>();

			try
			{
				await _context.SaveChangesAsync();
				serviceResponse.ResponseOk = bids;
			}
			catch (Exception e)
			{
				var errors = new List<EntityError>();
				errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
			}

			return serviceResponse;
		}

		public async Task<ServiceResponse<Bid, IEnumerable<EntityError>>> GetBid(string userId, int id)
		{
			var result = await _context.Bids.Where(b => b.Id == id && b.ApplicationUser.Id == userId).FirstOrDefaultAsync();
			var serviceResponse = new ServiceResponse<Bid, IEnumerable<EntityError>>();
			serviceResponse.ResponseOk = result;
			return serviceResponse;
		}

		public async Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteBids(int id)
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
			}

			return serviceResponse;
		}

		public bool BidsExists(string userId, int bidsId)
		{
			return _context.Bids.Any(e => e.Id == bidsId && e.ApplicationUser.Id == userId);
		}

        public async Task<ServiceResponse<double, IEnumerable<EntityError>>> GetBidForCar(int carId)
        {
		
			var serviceResponse = new ServiceResponse<double, IEnumerable<EntityError>>();
			try
			{
				var startBid = _context.Cars.Where(c => c.Id == carId).Sum(s => s.StartingBid);
				var bidForCar = _context.Bids.Where(c => c.Car.Id == carId);
				var value = await bidForCar.SumAsync(b => b.BidAmount); // return a task's result
				serviceResponse.ResponseOk = value + startBid;

			}
			catch (Exception e)
			{
				var errors = new List<EntityError>();
				errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
			}

			return serviceResponse;

			

        }
    }
}

