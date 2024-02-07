using AutoMapper;
using BookingService;
using Grpc.Core;
using PaymentService.Data;
using PaymentService.Models;

namespace PaymentService.SyncDataServices.Grpc
{
    public class GrpcPaymentService : GrpcPayment.GrpcPaymentBase
    {
        private readonly IPaymentRepository _repository;

        public GrpcPaymentService(IPaymentRepository repository)
        {
            _repository = repository;
        }

        public override Task<BookingResponse> SendPayment(GrpcBookingModel request, ServerCallContext context)
        {
            if (request == null)
            {
                Console.WriteLine("--> Incoming payment info is null");
                return Task.FromResult(new BookingResponse(null));
            }

            var response = new BookingResponse();
            try
            {
                if (_repository.GetPaymentByBookingId(request.BookingId) == null)
                {
                    Payment newPayment = new()
                    {
                        Status = "Pending",
                        StatusShort = "P",
                        PaymentTime = DateTime.Now,
                        BookingId = request.BookingId,
                    };
                    _repository.CreatePayment(newPayment);
                    _repository.SaveChanges();
                    response.Status = "Added";
                }
                else
                {
                    Console.WriteLine("--> Payment record for this Booking already exists!");
                    response.Status = "Already exists";
                }
                return Task.FromResult(response);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"--> Error while trying to add Payment info from remote call: {ex.Message}");
                return Task.FromResult(new BookingResponse(null));
            }
        }
    }
}
