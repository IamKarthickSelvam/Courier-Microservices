using BookingService.DTOs;

namespace BookingService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewBooking(BookingPublishedDto bookingPublishedDto);
    }
}
