using AtlasAir.Models;

namespace AtlasAir.Interfaces
{
    public interface IAirportRepository
    {
        Task CreateAsync(Airport airport);
        Task<Airport?> GetByIdAsync(int id);
        Task<List<Airport>?> GetAllAsync();
        Task UpdateAsync(Airport airport);
        Task DeleteAsync(Airport airport);
    }
}
