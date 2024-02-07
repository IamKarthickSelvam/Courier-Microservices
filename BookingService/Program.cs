using BookingService.AsyncDataServices;
using BookingService.Data;
using BookingService.SyncDataServices.Grpc;
using BookingService.SyncDataServices.Http;
using Microsoft.EntityFrameworkCore;

namespace BookingService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors();

            var env = builder.Environment.IsProduction() == true ? "Production" : "Development";
            Console.WriteLine($"--> Using Environment: {env}");
            Console.WriteLine("--> Using SqlServer Db");
                builder.Services.AddDbContext<AppDbContext>(opt =>
                    opt.UseSqlServer(builder.Configuration.GetConnectionString("CourierConn")));

            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddHttpClient<IHttpPaymentDataClient, HttpPaymentDataClient>();
            builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
            builder.Services.AddScoped<IPaymentDataClient, PaymentDataClient>();

            Console.WriteLine($"--> Payment Service Endpoint {builder.Configuration.GetValue<string>("PaymentService")}");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors(builder => builder.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:4200"));
                app.Use(async (HttpContext, next) =>
                {
                    HttpContext.Response.Headers.Append("Access-Control-Allow-Origin", "http://localhost:4200");
                    await next();
                });
            }
            else
            {
                app.UseCors(builder => builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            Console.WriteLine("--> Prepping DB...");
            PrepDb.PrepPopulation(app, true);

            app.Run();
        }
    }
}
