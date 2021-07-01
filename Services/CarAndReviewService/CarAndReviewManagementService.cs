using AutoMapper;
using CarShop.Data;
using CarShop.ErrorHandling;
using CarShop.Models;
using CarShop.ViewModels.AuctionEndViewModel;
using CarShop.ViewModels.CarsAndReviews;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Services.CarAndReviewService
{
    public class CarAndReviewManagementService : ICarAndReviewManagementService
    {
			public ApplicationDbContext _context;
			public CarAndReviewManagementService(ApplicationDbContext context)
			{
				_context = context;
			}

			public async Task<ServiceResponse<List<Car>, IEnumerable<EntityError>>> GetCars()
			{
				var cars = await _context.Cars.ToListAsync();
				var serviceResponse = new ServiceResponse<List<Car>, IEnumerable<EntityError>>();
				serviceResponse.ResponseOk = cars;
				return serviceResponse;
			}

			public async Task<ServiceResponse<Car, IEnumerable<EntityError>>> GetCar(int id)
			{
				var car = await _context.Cars.FindAsync(id);

				var serviceResponse = new ServiceResponse<Car, IEnumerable<EntityError>>();
				serviceResponse.ResponseOk = car;
				return serviceResponse;
			}

			public async Task<ServiceResponse<List<Review>, IEnumerable<EntityError>>> GetReviewsForCar(int id)
			{
				var reviews = await _context.Reviews.Where(r => r.CarId == id).ToListAsync();

				var serviceResponse = new ServiceResponse<List<Review>, IEnumerable<EntityError>>();
				serviceResponse.ResponseOk = reviews;
				return serviceResponse;
			}

			public async Task<ServiceResponse<Car, IEnumerable<EntityError>>> UpdateCar(Car car)
			{
				_context.Entry(car).State = EntityState.Modified;
				var serviceResponse = new ServiceResponse<Car, IEnumerable<EntityError>>();

				try
				{
					await _context.SaveChangesAsync();
					serviceResponse.ResponseOk = car;
				}
				catch (DbUpdateConcurrencyException e)
				{
					var errors = new List<EntityError>();
					errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
				}

				return serviceResponse;
			}

			public async Task<ServiceResponse<Review, IEnumerable<EntityError>>> UpdateReview(Review review)
			{
				_context.Entry(review).State = EntityState.Modified;
				var serviceResponse = new ServiceResponse<Review, IEnumerable<EntityError>>();

				try
				{
					await _context.SaveChangesAsync();

					serviceResponse.ResponseOk = review;
				}
				catch (DbUpdateConcurrencyException e)
				{
					var errors = new List<EntityError>();
					errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
				}

				return serviceResponse;
			}

			public async Task<ServiceResponse<Car, IEnumerable<EntityError>>> CreateCar(Car car)
			{
				_context.Cars.Add(car);
				var serviceResponse = new ServiceResponse<Car, IEnumerable<EntityError>>();

				try
				{
					await _context.SaveChangesAsync();
					serviceResponse.ResponseOk = car;
				}
				catch (Exception e)
				{
					var errors = new List<EntityError>();
					errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
				}

				return serviceResponse;
			}
			public async Task<ServiceResponse<Review, IEnumerable<EntityError>>> CreateReview(Review review)
			{
				_context.Reviews.Add(review);
				var serviceResponse = new ServiceResponse<Review, IEnumerable<EntityError>>();

				try
				{
					await _context.SaveChangesAsync();
					serviceResponse.ResponseOk = review;
				}
				catch (Exception e)
				{
					var errors = new List<EntityError>();
					errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
				}

				return serviceResponse;
			}

			public async Task<ServiceResponse<Review, IEnumerable<EntityError>>> AddReviewToCar(int carId, Review review)
			{
				var car = await _context.Cars
					.Where(c => c.Id == carId)
					.Include(c => c.Reviews).FirstOrDefaultAsync();

				var serviceResponse = new ServiceResponse<Review, IEnumerable<EntityError>>();

				if (car == null)
				{
					var errors = new List<EntityError>();
					errors.Add(new EntityError { Message = "The car doesn't exist." });
					return serviceResponse;
				}

				try
				{
					car.Reviews.Add(review);
					_context.Entry(car).State = EntityState.Modified;
					_context.SaveChanges();

					serviceResponse.ResponseOk = review;
				}
				catch (Exception e)
				{
					var errors = new List<EntityError>();
					errors.Add(new EntityError { ErrorType = e.GetType().ToString(), Message = e.Message });
				}

				return serviceResponse;
			}

			public async Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteReview(int reviewId)
			{
				var serviceResponse = new ServiceResponse<bool, IEnumerable<EntityError>>();

				try
				{
					var review = await _context.Reviews.FindAsync(reviewId);
					_context.Reviews.Remove(review);
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

			public async Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteCar(int carId)
			{
				var serviceResponse = new ServiceResponse<bool, IEnumerable<EntityError>>();

				try
				{
					var car = await _context.Cars.FindAsync(carId);
					_context.Cars.Remove(car);
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

			public bool CarExists(int id)
			{
				return _context.Cars.Any(e => e.Id == id);
			}

			public bool ReviewExists(int id)
			{
				return _context.Reviews.Any(e => e.Id == id);
			}

        public async Task<ServiceResponse<AuctionBill, IEnumerable<EntityError>>> CheckBidEnd(int carId)
        {

			var serviceResponse = new ServiceResponse<AuctionBill, IEnumerable<EntityError>>();


			try
			{
				var endDate = _context.Cars.SingleOrDefault(c => c.Id == carId).BidEnd;
				if (endDate > DateTime.Now)
                {

					var maxBid = _context.Bids.Where(b => b.Car.Id == carId).OrderByDescending(c => c.BidAmount).First();
					var auctionBill = new AuctionBill();
					var car = _context.Cars.SingleOrDefault(c => c.Id == carId);
					var user = _context.ApplicationUsers.SingleOrDefault(a => a.Id == maxBid.UserId);
					auctionBill.Car = car;
					auctionBill.User = user;
					auctionBill.CarSoldDate = DateTime.Now;
					_context.AuctionBills.Add(auctionBill);

					await _context.SaveChangesAsync();
					serviceResponse.ResponseOk = auctionBill;
                }

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

