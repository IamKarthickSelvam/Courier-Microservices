using Microsoft.EntityFrameworkCore;
using PaymentService.Models;

namespace PaymentService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Payment> Payments { get; set; }
    }
}
