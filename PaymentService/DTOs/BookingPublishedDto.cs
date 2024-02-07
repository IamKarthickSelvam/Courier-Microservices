namespace BookingService.DTOs
{
    public class BookingPublishedDto
    {
        public int Id { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        public DateTime BookedTime { get; set; }

        public string Event { get; set; }
    }
}
