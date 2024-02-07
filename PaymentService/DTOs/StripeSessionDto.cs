namespace PaymentService.DTOs
{
    public class StripeSessionDto
    {
        public string Id { get; set; }
        public string Url { get; set;}
        public int BookingId { get; set; }
    }
}
