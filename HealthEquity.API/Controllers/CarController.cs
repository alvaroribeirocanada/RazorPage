using AutoMapper;
using HealthEquity.API.Entities;
using HealthEquity.API.Models;
using HealthEquity.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthEquity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ICarService _service;
        private readonly IMapper _mapper;

        public CarController(ICarService service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service)); ;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<List<Car>>> GetCars()
        {
            var cars = await _service.GetCarsAsync();
            if (!cars.Succeeded)
            {
                return NotFound();
            }
            return Ok(cars.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCarById(int id)
        {
            var car = await _service.GetCarByIdAsync(id);
            if (!car.Succeeded)
            {
                return NotFound();
            }
            return Ok(car.Value);
        }

        [HttpPost]
        public async Task<ActionResult<CarModel>> AddCar(CarModel car)
        {
            var addedCar = await _service.AddCarAsync(_mapper.Map<Car>(car));
            if (!addedCar.Succeeded)
            {
                return BadRequest(addedCar.ErrorMessage);
            }
            return CreatedAtAction(nameof(GetCarById), new { id = addedCar.Value.Id }, addedCar.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, CarModel car)
        {
            var updated = await _service.UpdateCarAsync(id, _mapper.Map<Car>(car));
            if (!updated.Succeeded)
            {
                return BadRequest(updated.ErrorMessage);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            await _service.DeleteCarAsync(id);
            return NoContent();
        }
    }
}
