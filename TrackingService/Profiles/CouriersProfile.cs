using AutoMapper;
using BookingService.DTOs;
using PaymentService.DTOs;
using TrackingService.DTOs;
using TrackingService.Models;

namespace TrackingService.Profiles
{
    public class CouriersProfile : Profile
    {
        public CouriersProfile() 
        {
            // Source -> Target
            CreateMap<CourierCreateDto, Courier>();
            CreateMap<Courier, CourierReadDto>();
            CreateMap<BookingPublishedDto, Courier>()
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.From))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.To))
                .ForMember(dest => dest.BookingTime, opt => opt.MapFrom(src => src.BookedTime));
            CreateMap<PaymentPublishedDto, Courier>()
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.BookingId))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.StatusShort))
                .ForMember(dest => dest.PaymentTime, opt => opt.MapFrom(src => src.PaymentTime));
        }
    }
}
