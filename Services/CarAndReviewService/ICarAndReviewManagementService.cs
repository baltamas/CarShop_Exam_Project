using CarShop.ErrorHandling;
using CarShop.Models;
using CarShop.ViewModels.CarsAndReviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Services.CarAndReviewService
{
    public interface ICarAndReviewManagementService
    {
        public Task<ServiceResponse<List<Car>, IEnumerable<EntityError>>> GetCars();
        public Task<ServiceResponse<Car, IEnumerable<EntityError>>> GetCar(int id);
        public Task<ServiceResponse<List<Review>, IEnumerable<EntityError>>> GetReviewsForCar(int id);
        public Task<ServiceResponse<Review, IEnumerable<EntityError>>> CreateReview(Review review);
        public Task<ServiceResponse<Review, IEnumerable<EntityError>>> AddReviewToCar(int carId, Review review);
        public Task<ServiceResponse<Car, IEnumerable<EntityError>>> UpdateCar(Car car);
        public Task<ServiceResponse<Review, IEnumerable<EntityError>>> UpdateReview(Review review);
        public Task<ServiceResponse<Car, IEnumerable<EntityError>>> CreateCar(Car car);
        public Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteCar(int movieId);
        public Task<ServiceResponse<bool, IEnumerable<EntityError>>> DeleteReview(int reviewId);

        public bool ReviewExists(int id);
        bool CarExists(int id);
    }
}

