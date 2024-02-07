using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TrackingService.Models
{
    public class Courier
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int BookingId { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        public DateTime BookingTime { get; set; }

        public DateTime? PaymentTime { get; set;}

        public string PaymentStatus { get; set; }
    }
}
