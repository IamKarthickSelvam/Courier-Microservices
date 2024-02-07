using System.ComponentModel.DataAnnotations;

namespace PaymentService.DTOs
{
    public class PaymentCreateDto
    {
        [Required]
        public string Status { get; set; }

        [Required]
        public string StatusShort { get; set; }

        [Required]
        public DateTime PaymentTime { get; set; }

        [Required]
        public int BookingId { get; set; }
    }
}
