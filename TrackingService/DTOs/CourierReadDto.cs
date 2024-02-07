namespace TrackingService.DTOs
{
    public class CourierReadDto
    {
        public string? Id { get; set; }

        public int BookingId { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        public DateTime BookingTime { get; set; }

        public DateTime? PaymentTime { get; set; }

        public string? PaymentStatus { get; set; }
    }
}
