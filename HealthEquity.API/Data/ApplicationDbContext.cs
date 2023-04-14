using HealthEquity.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthEquity.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
    }
}
