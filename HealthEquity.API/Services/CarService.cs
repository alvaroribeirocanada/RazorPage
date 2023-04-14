using HealthEquity.API.Entities;
using HealthEquity.API.Models;
using HealthEquity.API.Repository;

namespace HealthEquity.API.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _repository;

        public CarService(ICarRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<Car>>> GetCarsAsync()
        {
            return Result<List<Car>>.Ok(await _repository.GetCarsAsync());
        }

        public async Task<Result<Car>> GetCarByIdAsync(int id)
        {
            var result = await _repository.GetCarByIdAsync(id);
            if (result == null)
            {
                return Result<Car>.NotFound("Car not found");
            }
            return Result<Car>.Ok(await _repository.GetCarByIdAsync(id));
        }

        public async Task<Result<Car>> AddCarAsync(Car car)
        {
            var validation = ValidateCar(car);

            if (!validation.Succeeded)
            {
                return Result<Car>.Error(validation.ErrorMessage);
            }

            return Result<Car>.Ok(await _repository.AddCarAsync(car));
        }        

        public async Task<Result<bool>> UpdateCarAsync(int id, Car car)
        {
            var validation = ValidateCar(car);

            if (!validation.Succeeded)
            {
                return Result<bool>.Error(validation.ErrorMessage);
            }

            await _repository.UpdateCarAsync(id, car);
            return Result<bool>.Ok(validation.Succeeded);
        }

        public async Task DeleteCarAsync(int id)
        {
            await _repository.DeleteCarAsync(id);
        }

        private Result<bool> ValidateCar(Car car)
        {
            if (car == null)
            {
                return Result<bool>.Error("Car must not be null");
            }

            if (string.IsNullOrWhiteSpace(car.Make))
            {
                return Result<bool>.Error("Make must not be null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(car.Model))
            {
                return Result<bool>.Error("Model must not be null or whitespace");
            }

            if (car.Year < 1900 || car.Year > DateTime.UtcNow.Year + 1) // Assume no cars can be from future
            {
                return Result<bool>.Error("Year must be between 1900 and next year");
            }

            if (car.Doors < 1 || car.Doors > 6) // Assume no cars can have more than 6 doors
            {
                return Result<bool>.Error("Doors must be between 1 and 6");
            }

            if (string.IsNullOrWhiteSpace(car.Color))
            {
                return Result<bool>.Error("Color must not be null or whitespace");
            }

            if (car.Price <= 0)
            {
                return Result<bool>.Error("Price must be greater than 0");
            }
            return Result<bool>.Ok(true);
        }
    }
}
