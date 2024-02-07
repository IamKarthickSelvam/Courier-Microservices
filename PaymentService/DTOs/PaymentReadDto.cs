namespace PaymentService.DTOs
{
    public class PaymentReadDto
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public string StatusShort { get; set; }

        public DateTime PaymentTime { get; set; }

        public int BookingId { get; set; }
    }
}
