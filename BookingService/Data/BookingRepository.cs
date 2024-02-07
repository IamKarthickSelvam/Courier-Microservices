using BookingService.DTOs;
using BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Data
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Pincode> GetAllPincodes()
        {
            return _context.Pincodes.ToList();
        }

        public IEnumerable<Booking> GetAllBookings()
        {
            return _context.Bookings.ToList();
        }

        public void CreateBooking(Booking bookingOrder)
        {
            _context.Bookings.Add(bookingOrder);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public Booking GetBookingById(int id)
        {
            return _context.Bookings.FirstOrDefault(b => b.Id == id);
        }
    }
}
