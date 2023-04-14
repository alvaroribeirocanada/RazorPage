using System.Collections.Generic;
using HealthEquity.API.Entities;
using HealthEquity.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthEquity.API.Pages
{
    public class ListCarsModel : PageModel
    {
        public List<Car> Cars { get; set; }

        public void OnGet()
        {
            Cars = new List<Car>
            {
                new Car { Id = 1, Make = "Audi", Model = "R8", Year = 2018, Doors = 2, Color = "Red", Price = 79995 },
                new Car { Id = 2, Make = "Tesla", Model = "3", Year = 2018, Doors = 4, Color = "Black", Price = 54995 },
                new Car { Id = 3, Make = "Porsche", Model = "911 991", Year = 2020, Doors = 2, Color = "White", Price = 155000 },
                new Car { Id = 4, Make = "Mercedes-Benz", Model = "GLE 63S", Year = 2021, Doors = 5, Color = "Blue", Price = 83995 },
                new Car { Id = 5, Make = "BMW", Model = "X6 M", Year = 2020, Doors = 5, Color = "Silver", Price = 62995 }
            };
        }
    }
}
