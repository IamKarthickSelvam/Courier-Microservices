using BookingService.DTOs;
using BookingService.Models;

namespace BookingService.Profiles
{
    public class BookingsProfile : AutoMapper.Profile
    {
        public BookingsProfile() 
        {
            // Source -> Target
            CreateMap<Pincode, PincodeReadDto>();
            CreateMap<BookingCreateDto, Booking>();
            CreateMap<Booking, BookingReadDto>();
            CreateMap<BookingReadDto, BookingPublishedDto>();
        }
    }
}
