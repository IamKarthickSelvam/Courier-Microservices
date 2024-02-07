using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TrackingService.Models;

namespace TrackingService.Data
{
    public class TrackingRepository : ITrackingRepository
    {
        private readonly IMongoCollection<Courier> _courierCollection;

        public TrackingRepository(IOptions<CourierDatabaseSettings> courierDatabaseSettings)
        {
            var mongoClient = new MongoClient(courierDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(courierDatabaseSettings.Value.DatabaseName);
            _courierCollection = mongoDatabase.GetCollection<Courier>(courierDatabaseSettings.Value.CourierCollectionName);
        }

        public async Task<List<Courier>> GetAsync() => await _courierCollection.Find(_ => true).ToListAsync();

        public async Task<Courier> GetByIdAsync(string id) => await _courierCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<Courier> GetByBookingIdAsync(int bookingId) => await _courierCollection.Find(x => x.BookingId == bookingId).FirstOrDefaultAsync();

        public async Task CreateAsync(Courier newCourier) => await _courierCollection.InsertOneAsync(newCourier);

        public async Task UpdateAsync(string id, Courier updatedCourier) => await _courierCollection.ReplaceOneAsync(x => x.Id == id, updatedCourier);

        public async Task RemoveAsync(string id) => await _courierCollection.DeleteOneAsync(x => x.Id == id);
    }
}
