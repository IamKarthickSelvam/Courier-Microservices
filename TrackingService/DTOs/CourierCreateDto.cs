namespace TrackingService.DTOs
{
    public class CourierCreateDto
    {
        public int BookingId { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        public DateTime BookingTime { get; set; }

        public DateTime? PaymentTime { get; set; }

        public string PaymentStatus { get; set; }
    }
}
