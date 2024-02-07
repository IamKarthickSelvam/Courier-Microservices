namespace BookingService.DTOs
{
    public class BookingReadDto
    {
        public int Id { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        public DateTime BookedTime { get; set; }

        public int Weight { get; set; }

        public string ContentType { get; set; }
    }
}
