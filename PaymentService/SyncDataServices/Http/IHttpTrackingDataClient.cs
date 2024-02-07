using PaymentService.DTOs;

namespace PaymentService.SyncDataServices.Http
{
    public interface IHttpTrackingDataClient
    {
        Task SendPaymentToTracking(PaymentReadDto payment);
    }
}
