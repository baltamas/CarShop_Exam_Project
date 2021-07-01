using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarShop.Data;
using CarShop.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;
using CarShop.Services.CarAndReviewService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CarShop.Services;
using CarShop.ViewModels.CarsAndReviews;
using CarShop.ViewModels.AuctionEndViewModel;

namespace CarShop.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[Produces("application/json")]
	public class CarController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ICarAndReviewManagementService _carAndReviewManagementService;

		public CarController(IMapper mapper, ICarAndReviewManagementService carAndReviewManagementService)
		{
			_mapper = mapper;
			_carAndReviewManagementService = carAndReviewManagementService;
		}

		/// <summary>
		/// Retrieves a list of cars.
		/// <response code="200">The cars.</response>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CarViewModel>>> GetCars()
		{
			var carsResponse = await _carAndReviewManagementService.GetCars();
			var cars = carsResponse.ResponseOk;

			return _mapper.Map<List<Car>, List<CarViewModel>>(cars);
		}

		/// <summary>
		/// Retrieves a car by ID, including its reviews.
		/// </summary>
		/// <param name="id">The car ID</param>
		/// <response code="200">The car.</response>
		/// <response code="404">If the car is not found.</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpGet("{id}/Reviews")]
		public async Task<ActionResult<CarWithReviewViewModel>> GetReviewsForCar(int id)
		{
			if (!_carAndReviewManagementService.CarExists(id))
			{
				return NotFound();
			}

			var carResponse = await _carAndReviewManagementService.GetCar(id);
			var car = carResponse.ResponseOk;

			if (car == null)
			{
				return NotFound();
			}

			var reviewsResponse = await _carAndReviewManagementService.GetReviewsForCar(id);
			var reviews = reviewsResponse.ResponseOk;

			var result = _mapper.Map<CarWithReviewViewModel>(car);
			result.Reviews = _mapper.Map<List<Review>, List<ReviewViewModel>>(reviews);

			return result;
		}

		/// <summary>
		/// Retrieves a car by ID.
		/// </summary>
		/// <param name="id">The car ID</param>
		/// <response code="200">The car.</response>
		/// <response code="404">If the car is not found.</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<ActionResult<CarViewModel>> GetCar(int id)
		{
			var carResponse = await _carAndReviewManagementService.GetCar(id);
			var car = carResponse.ResponseOk;

			if (car == null)
			{
				return NotFound();
			}

			return _mapper.Map<CarViewModel>(car);
		}

		/// <summary>
		/// Updates a Car.
		/// <param name="id">The car ID</param>
		/// <param name="car">The car body.</param>
		/// <response code="204">If the item was successfully added.</response>
		/// <response code="400">If the ID in the URL doesn't match the one in the body.</response>
		/// <response code="404">If the item is not found.</response>
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpPut("{id}")]
		[Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
		public async Task<IActionResult> PutCar(int id, CarViewModel car)
		{
			if (id != car.Id)
			{
				return BadRequest();
			}

			var carResponse = await _carAndReviewManagementService.UpdateCar(_mapper.Map<Car>(car));

			if (carResponse.ResponseError == null)
			{
				return NoContent();
			}

			if (!_carAndReviewManagementService.CarExists(id))
			{
				return NotFound();
			}

			return StatusCode(500);
		}

		/// <summary>
		/// Updates a car review.
		/// </summary>
		/// <remarks>
		/// Sample request:
		/// POST /api/Car/3/Reviews
		/// {
		///		"Id":4,
		///    "Content": "some text",
		///    "DateTime": "2021-07-02T10:50:00",
		///    "carId": 3,
		/// </remarks>
		/// <param name="reviewId">The review ID</param>
		/// <param name="review">The review body</param>
		/// <response code="204">If the item was successfully added.</response>
		/// <response code="400">If the ID in the URL doesn't match the one in the body.</response>
		/// <response code="404">If the item is not found.</response>
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpPut("{id}/Reviews/{reviewId}")]
		public async Task<IActionResult> PutReview(int reviewId, ReviewViewModel review)
		{
			if (reviewId != review.Id)
			{
				return BadRequest();
			}

			if (!_carAndReviewManagementService.CarExists(review.CarId))
			{
				return NotFound();
			}

			var reviewResponse = await _carAndReviewManagementService.UpdateReview(_mapper.Map<Review>(review));

			if (reviewResponse.ResponseError == null)
			{
				return NoContent();
			}

			if (!_carAndReviewManagementService.ReviewExists(reviewId))
			{
				return NotFound();
			}

			return StatusCode(500);
		}

		// POST: api/Movies
		/// <summary>
		/// Creates a car.
		/// </summary>
		/// <param name="car"></param>
		/// <response code="201">Returns the newly created item</response>
		/// <response code="400">If the item is null</response>
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpPost]
		[Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
		public async Task<ActionResult<Car>> PostCar(CarViewModel car)
		{
			var carResponse = await _carAndReviewManagementService.CreateCar(_mapper.Map<Car>(car));

			if (carResponse.ResponseError == null)
			{
				return CreatedAtAction("GetCar", new { id = car.Id }, car);
			}

			return StatusCode(500);
		}

		/// <summary>
		/// Creates a car review.
		/// </summary>
		/// <param name="id">The car ID</param>
		/// <param name="review">The review body</param>
		/// <response code="200">If the item was successfully added.</response>
		/// <response code="404">If car is not found.</response>  
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpPost("{id}/Reviews")]
		public async Task<IActionResult> PostReviewForCar(int id, ReviewViewModel review)
		{
			var reviewResponse = await _carAndReviewManagementService.AddReviewToCar(id, _mapper.Map<Review>(review));

			if (reviewResponse.ResponseError == null)
			{
				return Ok();
			}

			return StatusCode(500);
		}

		/// <summary>
		/// Deletes a car.
		/// </summary>
		/// <param name="id"></param>
		/// <response code="204">No content if successful.</response>
		/// <response code="404">If the car doesn't exist.</response>  
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpDelete("{id}")]
		[Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
		public async Task<IActionResult> DeleteCar(int id)
		{
			if (!_carAndReviewManagementService.CarExists(id))
			{
				return NotFound();
			}

			var result = await _carAndReviewManagementService.DeleteCar(id);

			if (result.ResponseError == null)
			{
				return NoContent();
			}


			return StatusCode(500);
		}


		// DELETE: api/Car/1/Reviews/5
		/// <summary>
		/// Deletes a car review.
		/// </summary>
		/// <remarks>
		/// Sample request:
		///
		/// DELETE api/Car/1/Reviews/5
		///
		/// </remarks>
		/// <param name="reviewId"></param>
		/// <response code="204">No content if successful.</response>
		/// <response code="404">If the review doesn't exist.</response>  
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpDelete("{id}/Reviews/{reviewId}")]
		[Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
		public async Task<IActionResult> DeleteReview(int reviewId)
		{
			if (!_carAndReviewManagementService.ReviewExists(reviewId))
			{
				return NotFound();
			}

			var result = await _carAndReviewManagementService.DeleteReview(reviewId);

			if (result.ResponseError == null)
			{
				return NoContent();
			}


			return StatusCode(500);
		}

		[HttpPut]
		public async Task <ActionResult<AuctionBillViewModel>> CheckBidEnd(int carId)
        {
			
			var result = await _carAndReviewManagementService.CheckBidEnd(carId);

			var auctionBill = result.ResponseOk;
			var auctionBillViewModel = new AuctionBillViewModel(auctionBill);
			if (result.ResponseError == null)
            {
				return auctionBillViewModel;
            }

			return StatusCode(500);
        }

	}
}

    
