namespace BookingService.SyncDataServices.Http
{
    public interface IHttpPaymentDataClient
    {
        Task SendBookingToPayment(int id);
    }
}
