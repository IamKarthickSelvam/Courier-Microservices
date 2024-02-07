
using TrackingService.AsyncDataServices;
using TrackingService.Data;
using TrackingService.EventProcessing;
using TrackingService.Models;

namespace TrackingService
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
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.Configure<CourierDatabaseSettings>(builder.Configuration.GetSection("CourierDatabase"));
            builder.Services.AddSingleton<ITrackingRepository, TrackingRepository>();
            builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
            builder.Services.AddHostedService<MessageBusSubscriber>();

            var env = builder.Environment.IsProduction() == true ? "Production" : "Development";
            Console.WriteLine($"--> Using Environment: {env}");

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

            app.Run();
        }
    }
}
