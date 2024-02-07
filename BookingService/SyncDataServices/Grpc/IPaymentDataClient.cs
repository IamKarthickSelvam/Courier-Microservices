using BookingService.Models;

namespace BookingService.SyncDataServices.Grpc
{
    public interface IPaymentDataClient
    {
        void InitiatePayment(GrpcBookingModel grpcBookingModel);
    }
}
