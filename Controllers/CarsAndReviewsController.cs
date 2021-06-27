using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarShop.Data;
using CarShop.Models;
using CarShop.ViewModels;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace CarShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsAndReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CarsAndReviewsController> _logger;
        private readonly IMapper _mapper;

        public CarsAndReviewsController(ApplicationDbContext context, ILogger<CarsAndReviewsController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns cars with a minimum price
        /// </summary>
        /// <param name="minPrice"></param>
        /// <returns>A list of cars with price >= by the give minprice</returns>
        // GET: api/Cars
        [HttpGet]
        [Route("filterCarPrice/{minPrice}")]
        public ActionResult<IEnumerable<Car>> FilterMinPrice(int minPrice)
        {
            var query = _context.Cars.Where(c => c.Price >= minPrice);
            _logger.LogInformation(query.ToQueryString());
            return _context.Cars.Where(c => c.Price >= minPrice).ToList();

        }

        // GET: api/Cars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            return await _context.Cars.ToListAsync();
        }

        // GET: api/Cars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CarViewModel>> GetCar(int id)
        {
            var car = await _context.Cars.FindAsync(id); 

            if (car == null)
            {
                return NotFound();
            }

            var carViewModel = _mapper.Map<CarViewModel>(car);

            return carViewModel;
        }

        // PUT: api/Cars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(int id, Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCar", new { id = car.Id }, car);
        }

        // DELETE: api/Cars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }

        //Controller for reviews!
        [HttpGet("{id}/Reviews")]
        public ActionResult<IEnumerable<CarWithReviewViewModel>> GetReviewForCar(int id)
        {
            var query = _context.Reviews.Where(r => r.Car.Id == id).Include(r => r.Car).Select(r => new CarWithReviewViewModel
            {
                Id = r.Car.Id,
                Name = r.Car.Name,
                Description = r.Car.Description,
                Price = r.Car.Price,
                MileAge = r.Car.MileAge,
                Year = r.Car.Year,
                Reviews = r.Car.Reviews.Select(cr => new ReviewViewModel
                {
                    Id = cr.Id,
                    Content = cr.Content,
                    DateTime = cr.DateTime
                })
            });

           
            var query_v2 = _context.Cars.Where(c => c.Id == id).Include(c=> c.Reviews).Select(c => _mapper.Map<CarWithReviewViewModel>(c));
             _logger.LogInformation(query.ToQueryString());

            var queryForReviewCarId = _context.Reviews;
            _logger.LogInformation(queryForReviewCarId.ToList()[0].CarId.ToString());

            return query_v2.ToList();
        }

        [HttpPost("{id}/Reviews")]
        public IActionResult PostReviewForCar (int id, Review review)
        {
            var car = _context.Cars.Where(c => c.Id == id).Include(c => c.Reviews).FirstOrDefault();
            if (car == null)
            {
                return NotFound();

            }
            car.Reviews.Add(review);
            _context.Entry(car).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("{id}/Reviews/{ReviewId}")]
        public async Task<IActionResult> PutReview(int reviewId, ReviewViewModel review )
        {
            if (reviewId != review.Id)
            {
                return BadRequest();
            }

            _context.Entry(_mapper.Map<Review>(review)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(reviewId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}/Comments/{commentId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool ReviewExists(int id)
        {
            return _context.Reviews .Any(c => c.Id == id);
        }
    }
}

    
