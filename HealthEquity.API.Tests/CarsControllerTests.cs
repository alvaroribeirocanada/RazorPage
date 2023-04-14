using AutoMapper;
using HealthEquity.API.Controllers;
using HealthEquity.API.Entities;
using HealthEquity.API.Models;
using HealthEquity.API.Repository;
using HealthEquity.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HealthEquity.API.Tests
{
    public class CarsControllerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICarRepository> _mockRepository;
        private readonly CarService _service;
        private readonly CarController _controller;

        public CarsControllerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<ICarRepository>();
            _service = new CarService(_mockRepository.Object);
            _controller = new CarController(_service, _mockMapper.Object);
        }

        [Fact]
        public async Task GetCars_ReturnsOk_WithListOfCars()
        {
            // Arrange
            var car = new List<Car> { new Car { Id = 1, Make = "Audi", Model = "R8" } };

            _mockRepository.Setup(s => s.GetCarsAsync()).ReturnsAsync(car);
            
            // Act
            var result = await _controller.GetCars();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualCars = Assert.IsAssignableFrom<List<Car>>(okResult.Value);
            Assert.Equal(car, actualCars);
        }

        [Fact]
        public async Task GetCarById_ReturnsNotFound_WhenCarNotFound()
        {
            // Arrange
            var id = 1;
            _mockRepository.Setup(s => s.GetCarByIdAsync(id)).ReturnsAsync(null as Car);

            // Act
            var result = await _controller.GetCarById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetCarById_ReturnsOk_WithCar()
        {
            // Arrange
            var id = 1;
            var car = new Car { Id = 1, Make = "Audi", Model = "R8" };
            _mockRepository.Setup(s => s.GetCarByIdAsync(id)).ReturnsAsync(car);

            // Act
            var result = await _controller.GetCarById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualCar = Assert.IsAssignableFrom<Car>(okResult.Value);
            Assert.Equal(car, actualCar);
        }

        [Fact]
        public async Task AddCar_ReturnsCreatedAtAction_WithNewCar()
        {
            // Arrange
            var carModel = new CarModel { Make = "Audi", Model = "R8", Year = 2018, Doors = 2, Color = "Red", Price = 79995 };
            var car = new Car { Make = "Audi", Model = "R8", Year = 2018, Doors = 2, Color = "Red", Price = 79995 };
            _mockRepository.Setup(s => s.AddCarAsync(car)).ReturnsAsync(car);
            _mockMapper.Setup(s => s.Map<Car>(carModel)).Returns(car);

            // Act
            var result = await _controller.AddCar(carModel);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(CarController.GetCarById), createdAtActionResult.ActionName);
            Assert.Equal(car.Id, createdAtActionResult.RouteValues["id"]);
            var actualCar = Assert.IsAssignableFrom<Car>(createdAtActionResult.Value);
            Assert.Equal(carModel.Model, actualCar.Model);
            Assert.Equal(carModel.Make, actualCar.Make);
        }

        [Fact]
        public async Task AddCar_ReturnsBadRequest_InvalidMake()
        {
            // Arrange
            var carModel = new CarModel();
            var car = new Car();
            _mockRepository.Setup(s => s.AddCarAsync(car)).ReturnsAsync(car);
            _mockMapper.Setup(s => s.Map<Car>(carModel)).Returns(car);

            // Act
            var result = await _controller.AddCar(carModel);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(badRequestObjectResult.Value, "Make must not be null or whitespace");

        }

        [Fact]
        public async Task AddCar_ReturnsBadRequest_InvalidModel()
        {
            // Arrange
            var carModel = new CarModel { Make = "Ford"};
            var car = new Car { Make = "Ford" };
            _mockRepository.Setup(s => s.AddCarAsync(car)).ReturnsAsync(car);
            _mockMapper.Setup(s => s.Map<Car>(carModel)).Returns(car);

            // Act
            var result = await _controller.AddCar(carModel);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(badRequestObjectResult.Value, "Model must not be null or whitespace");
        }

        [Fact]
        public async Task AddCar_ReturnsBadRequest_InvalidYear()
        {
            // Arrange
            var carModel = new CarModel { Make = "Ford", Model = "Focus" };
            var car = new Car { Make = "Ford", Model = "Focus" };
            _mockRepository.Setup(s => s.AddCarAsync(car)).ReturnsAsync(car);
            _mockMapper.Setup(s => s.Map<Car>(carModel)).Returns(car);

            // Act
            var result = await _controller.AddCar(carModel);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(badRequestObjectResult.Value, "Year must be between 1900 and next year");
        }

        [Fact]
        public async Task AddCar_ReturnsBadRequest_InvalidDoors()
        {
            // Arrange
            var carModel = new CarModel { Make = "Ford", Model = "Focus", Year = 2008 };
            var car = new Car { Make = "Ford", Model = "Focus", Year = 2008 };
            _mockRepository.Setup(s => s.AddCarAsync(car)).ReturnsAsync(car);
            _mockMapper.Setup(s => s.Map<Car>(carModel)).Returns(car);

            // Act
            var result = await _controller.AddCar(carModel);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(badRequestObjectResult.Value, "Doors must be between 1 and 6");
        }

        [Fact]
        public async Task AddCar_ReturnsBadRequest_InvalidColor()
        {
            // Arrange
            var carModel = new CarModel { Make = "Ford", Model = "Focus", Year = 2008, Doors = 2 };
            var car = new Car { Make = "Ford", Model = "Focus", Year = 2008, Doors = 2 };
            _mockRepository.Setup(s => s.AddCarAsync(car)).ReturnsAsync(car);
            _mockMapper.Setup(s => s.Map<Car>(carModel)).Returns(car);

            // Act
            var result = await _controller.AddCar(carModel);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(badRequestObjectResult.Value, "Color must not be null or whitespace");
        }

        [Fact]
        public async Task AddCar_ReturnsBadRequest_InvalidPrice()
        {
            // Arrange
            var carModel = new CarModel { Make = "Ford", Model = "Focus", Year = 2008, Doors = 2, Color = "Red" };
            var car = new Car { Make = "Ford", Model = "Focus", Year = 2008, Doors = 2, Color = "Red" };
            _mockRepository.Setup(s => s.AddCarAsync(car)).ReturnsAsync(car);
            _mockMapper.Setup(s => s.Map<Car>(carModel)).Returns(car);

            // Act
            var result = await _controller.AddCar(carModel);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(badRequestObjectResult.Value, "Price must be greater than 0");
        }

        [Fact]
        public async Task UpdateCar_ReturnsNoContent_WhenCarUpdated()
        {
            // Arrange
            var id = 1;
            var carModel = new CarModel { Make = "Ford", Model = "Focus", Year = 2008, Doors = 2, Color = "Red", Price = 15000 };
            var car = new Car { Make = "Ford", Model = "Focus", Year = 2008, Doors = 2, Color = "Red", Price = 15000 };
            _mockMapper.Setup(s => s.Map<Car>(carModel)).Returns(car);

            // Act
            var result = await _controller.UpdateCar(id, carModel);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateCar_ReturnsBadRequest_InvalidMake()
        {
            // Arrange
            var id = 1;
            var carModel = new CarModel();
            var car = new Car();
            _mockMapper.Setup(s => s.Map<Car>(carModel)).Returns(car);

            // Act
            var result = await _controller.UpdateCar(id, carModel);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(badRequestObjectResult.Value, "Make must not be null or whitespace");
        }

        [Fact]
        public async Task UpdateCar_ReturnsBadRequest_InvalidModel()
        {
            // Arrange
            var id = 1;
            var carModel = new CarModel { Make = "Ford" };
            var car = new Car { Make = "Ford" };
            _mockMapper.Setup(s => s.Map<Car>(carModel)).Returns(car);

            // Act
            var result = await _controller.UpdateCar(id, carModel);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(badRequestObjectResult.Value, "Model must not be null or whitespace");
        }

        [Fact]
        public async Task UpdateCar_ReturnsBadRequest_InvalidYear()
        {
            // Arrange
            var id = 1;
            var carModel = new CarModel { Make = "Ford", Model = "Focus" };
            var car = new Car { Make = "Ford", Model = "Focus" };
            _mockMapper.Setup(s => s.Map<Car>(carModel)).Returns(car);

            // Act
            var result = await _controller.UpdateCar(id, carModel);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(badRequestObjectResult.Value, "Year must be between 1900 and next year");
        }

        [Fact]
        public async Task UpdateCar_ReturnsBadRequest_InvalidDoors()
        {
            // Arrange
            var id = 1;
            var carModel = new CarModel { Make = "Ford", Model = "Focus", Year = 2008 };
            var car = new Car { Make = "Ford", Model = "Focus", Year = 2008 };
            _mockMapper.Setup(s => s.Map<Car>(carModel)).Returns(car);

            // Act
            var result = await _controller.UpdateCar(id, carModel);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(badRequestObjectResult.Value, "Doors must be between 1 and 6");
        }

        [Fact]
        public async Task UpdateCar_ReturnsBadRequest_InvalidColor()
        {
            // Arrange
            var id = 1;
            var carModel = new CarModel { Make = "Ford", Model = "Focus", Year = 2008, Doors = 2 };
            var car = new Car { Make = "Ford", Model = "Focus", Year = 2008, Doors = 2 };
            _mockMapper.Setup(s => s.Map<Car>(carModel)).Returns(car);

            // Act
            var result = await _controller.UpdateCar(id, carModel);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(badRequestObjectResult.Value, "Color must not be null or whitespace");
        }

        [Fact]
        public async Task UpdateCar_ReturnsBadRequest_InvalidPrice()
        {
            // Arrange
            var id = 1;
            var carModel = new CarModel { Make = "Ford", Model = "Focus", Year = 2008, Doors = 2, Color = "Red" };
            var car = new Car { Make = "Ford", Model = "Focus", Year = 2008, Doors = 2, Color = "Red" };
            _mockMapper.Setup(s => s.Map<Car>(carModel)).Returns(car);

            // Act
            var result = await _controller.UpdateCar(id, carModel);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(badRequestObjectResult.Value, "Price must be greater than 0");
        }

        [Fact]
        public async Task DeleteCar_ReturnsNoContent_WhenCarDeleted()
        {
            // Arrange
            int carId = 1;
            _mockRepository.Setup(s => s.DeleteCarAsync(carId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCar(carId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }

    }
}