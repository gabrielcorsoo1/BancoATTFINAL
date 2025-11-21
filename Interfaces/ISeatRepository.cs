using AtlasAir.Models;

namespace AtlasAir.Interfaces
{
    public interface ISeatRepository
    {
        Task CreateAsync(Seat seat);
        Task<Seat?> GetByIdAsync(int id);
        Task<List<Seat>?> GetAllAsync();
        Task UpdateAsync(Seat seat);
        Task DeleteAsync(Seat seat);
        Task<List<Seat>?> GetAvailableSeatsByFlightIdAsync(int flightId);
    }
}
