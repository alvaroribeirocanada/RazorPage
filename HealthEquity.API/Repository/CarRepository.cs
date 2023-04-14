using HealthEquity.API.Data;
using HealthEquity.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthEquity.API.Repository
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Car>> GetCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            return await _context.Cars.FindAsync(id);
        }

        public async Task<Car> AddCarAsync(Car car)
        {
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
            return car;
        }

        public async Task UpdateCarAsync(int id, Car car)
        {
            var carToUpdate = await _context.Cars.FindAsync(id);
            if (carToUpdate != null)
            {
                carToUpdate.Price = car.Price;
                carToUpdate.Color = car.Color;
                carToUpdate.Make = car.Make;
                carToUpdate.Model = car.Model;
                carToUpdate.Year = car.Year;
                carToUpdate.Doors = car.Doors;
                _context.Cars.Update(carToUpdate);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
        }
    }
}
