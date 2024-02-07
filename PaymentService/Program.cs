using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.SyncDataServices.Http;
using PaymentService.SyncDataServices.Grpc;
using PaymentService.Models;
using PaymentService.AsyncDataServices;

namespace PaymentService
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
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
            builder.Services.AddHttpClient<IHttpTrackingDataClient, HttpTrackingDataClient>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddGrpc();

            var env = builder.Environment.IsProduction() == true ? "Production" : "Development";
            Console.WriteLine($"--> Using Environment: {env}");
            Console.WriteLine("--> Using Sqlite Db");
            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlite(builder.Configuration.GetConnectionString("CourierSqlite")));

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

            app.MapGrpcService<GrpcPaymentService>();

            //optional
            app.MapGet("/protos/platforms/proto", async context =>
            {
                await context.Response.WriteAsync(File.ReadAllText("Protos/bookings.proto"));
            });

            Console.WriteLine(builder.Environment.IsProduction());
            PrepDb.PrepPopulation(app, builder.Environment.IsProduction());

            app.Run();
        }
    }
}
