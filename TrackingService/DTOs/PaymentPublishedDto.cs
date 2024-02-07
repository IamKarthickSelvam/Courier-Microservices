namespace PaymentService.DTOs
{
    public class PaymentPublishedDto
    {
        public int Id { get; set; }

        public string StatusShort { get; set; }

        public DateTime PaymentTime { get; set; }

        public int BookingId { get; set; }

        public string Event { get; set; }
    }
}
