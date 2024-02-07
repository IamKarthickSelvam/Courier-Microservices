using AutoMapper;
using BookingService.Models;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BookingService.SyncDataServices.Grpc
{
    public class PaymentDataClient : IPaymentDataClient
    {
        private readonly IConfiguration _configuration;

        public PaymentDataClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void InitiatePayment(GrpcBookingModel grpcBookingModel)
        {
            Console.WriteLine($"--> Calling gRPC Service {_configuration["GrpcPlatform"]}");
            var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]);
            var client = new GrpcPayment.GrpcPaymentClient(channel);
            var request = grpcBookingModel;

            try
            {
                var reply = client.SendPayment(request);

                var consoleMessage = "";
                switch (reply.Status)
                {
                    case "Added":
                        consoleMessage = "--> Added Successfully";
                        break;
                    case "Already exists":
                        consoleMessage = "--> Payment record for this Booking already exists!";
                        break;
                    default:
                        consoleMessage = null;
                        break;
                }
                Console.WriteLine(consoleMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not call gRPC Server {ex.Message}");
            }
        }
    }
}
