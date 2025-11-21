using AtlasAir.Models;

namespace AtlasAir.Interfaces
{
    public interface IReservationRepository
    {
        Task CreateAsync(Reservation reservation);
        Task<Reservation?> GetByIdAsync(int id);
        Task<List<Reservation>?> GetAllAsync();
        Task UpdateAsync(Reservation reservation);
        Task DeleteAsync(Reservation reservation);
    }
}
