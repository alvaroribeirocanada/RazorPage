using HealthEquity.API.Entities;
using HealthEquity.API.Models;

namespace HealthEquity.API.Services
{
    public interface ICarService
    {
        Task<Result<List<Car>>> GetCarsAsync();
        Task<Result<Car>> GetCarByIdAsync(int id);
        Task<Result<Car>> AddCarAsync(Car car);
        Task<Result<bool>> UpdateCarAsync(int id, Car car);
        Task DeleteCarAsync(int id);
    }
}
