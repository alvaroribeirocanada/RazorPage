using HealthEquity.API.Entities;

namespace HealthEquity.API.Repository
{
    public interface ICarRepository
    {
        Task<List<Car>> GetCarsAsync();
        Task<Car> GetCarByIdAsync(int id);
        Task<Car> AddCarAsync(Car car);
        Task UpdateCarAsync(int id, Car car);
        Task DeleteCarAsync(int id);
    }
}
