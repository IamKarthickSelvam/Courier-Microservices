using Microsoft.EntityFrameworkCore;

namespace BookingService.Data
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

            if (!context.Pincodes.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                context.Pincodes.AddRange(
                    new Models.Pincode() { Name = "Chennai", Code = 600001 },
                    new Models.Pincode() { Name = "Chengalpattu", Code = 600127 }
                    );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> Data available already!");
            }
        }
    }
}
