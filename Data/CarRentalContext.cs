using Biluthyrning.Models;
using Microsoft.EntityFrameworkCore;

namespace Biluthyrning.Data
{
    public class CarRentalContext : DbContext
    {
        public CarRentalContext(DbContextOptions<CarRentalContext> options) : base(options)
        {

        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarCategory> Categories { get; set; }
        public DbSet<User> Users { get; set; }

    }
}