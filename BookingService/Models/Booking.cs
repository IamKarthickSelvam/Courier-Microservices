using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService.Models
{
    public class Booking
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int From { get; set; }

        [Required]
        public int To { get; set; }

        [Required]
        public DateTime BookedTime { get; set; }

        [Required]
        public int Weight  { get; set; }

        [Required]
        public string ContentType { get; set; }
    }
}
