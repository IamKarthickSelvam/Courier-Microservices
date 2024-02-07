using TrackingService.Models;

namespace TrackingService.Data
{
    public interface ITrackingRepository
    {
        Task<List<Courier>> GetAsync();
        Task<Courier> GetByIdAsync(string id);
        Task<Courier> GetByBookingIdAsync(int bookingId);
        Task CreateAsync(Courier newCourier);
        Task UpdateAsync(string id, Courier updatedCourier);
        Task RemoveAsync(string id);
    }
}
