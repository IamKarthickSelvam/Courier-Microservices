using PaymentService.DTOs;
using PaymentService.Models;

namespace PaymentService.Profiles
{
    public class PaymentsProfile : AutoMapper.Profile
    {
        public PaymentsProfile() 
        {
            // Source -> Target
            CreateMap<Payment, PaymentReadDto>();
        }
    }
}
