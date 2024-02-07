using BookingService.Models;

namespace BookingService.Data
{
    public interface IBookingRepository
    {
        bool SaveChanges();
        IEnumerable<Pincode> GetAllPincodes();
        Booking GetBookingById(int id);
        IEnumerable<Booking> GetAllBookings();
        void CreateBooking(Booking bookingOrder);
    }
}
