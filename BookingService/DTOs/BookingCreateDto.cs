using System.ComponentModel.DataAnnotations;

namespace BookingService.DTOs
{
    public class BookingCreateDto
    {
        [Required]
        public int From { get; set; }

        [Required]
        public int To { get; set; }

        [Required]
        public DateTime BookedTime { get; set; }

        [Required]
        public int Weight { get; set; }

        [Required]
        public string ContentType { get; set; }
    }
}
