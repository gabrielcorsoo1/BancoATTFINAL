using AtlasAir.Models;

namespace AtlasAir.Interfaces
{
    public interface IAircraftRepository
    {
        Task CreateAsync(Aircraft aircraft);
        Task<Aircraft?> GetByIdAsync(int id);
        Task<List<Aircraft>?> GetAllAsync();
        Task UpdateAsync(Aircraft aircraft);
        Task DeleteAsync(Aircraft aircraft);
    }
}
