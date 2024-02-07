using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentService.Models
{
    public class Payment
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        [Column(TypeName = "text")]
        public string Status { get; set; }

        [Required]
        [MaxLength(2)]
        [Column(TypeName = "text")]
        public string StatusShort { get; set; }

        [Required]
        public DateTime PaymentTime { get; set; }

        [Required]
        public int BookingId { get; set; }
    }
}
