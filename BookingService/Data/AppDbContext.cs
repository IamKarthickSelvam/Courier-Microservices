using BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Pincode> Pincodes { get; set; }
    }
}
