using Microsoft.EntityFrameworkCore;
using PaymentService.Models;

namespace PaymentService.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd = true)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }

            if (!context.Payments.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                context.Payments.AddRange(
                    new Payment() { Status = "Pending", StatusShort = "P", PaymentTime = DateTime.Now, BookingId = 5 },
                    new Payment() { Status = "Done", StatusShort = "D", PaymentTime = DateTime.Now.AddDays(2), BookingId = 6 }
                    );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
