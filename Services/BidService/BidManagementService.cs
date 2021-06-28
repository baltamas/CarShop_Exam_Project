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
			List<Car> cars = new List<Car>();

			newBidRequest.BidCarIds.ForEach(cid =>
			{
				var car = _context.Cars.Find(cid);
				if (car != null)
				{
					cars.Add(car);
				}
			});

			var bid = new Bid
			{
				UserId = userId,
				Cars = cars,
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
			var result = await _context.Bids.Where(b => b.ApplicationUser.Id == userId).Include(b => b.Cars).OrderByDescending(c => c.BidAmount).ToListAsync();
			var serviceResponse = new ServiceResponse<List<Bid>, IEnumerable<EntityError>>();
			serviceResponse.ResponseOk = result;
			return serviceResponse;
		}

		public async Task<ServiceResponse<Bid, IEnumerable<EntityError>>> UpdateBids(Bid bids, UpdateBidForUser updateBidForUser)
		{
			bids.Cars = await _context.Cars.Where(c => updateBidForUser.CarIds.Contains(c.Id)).ToListAsync();
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
	}
}

