using AtlasAir.Models;

namespace AtlasAir.Interfaces
{
    public interface IFlightRepository
    {
        Task CreateAsync(Flight flight);
        Task<Flight?> GetByIdAsync(int id);
        Task<List<Flight>?> GetAllAsync();
        Task UpdateAsync(Flight flight);
        Task DeleteAsync(Flight flight);
        Task<List<Flight>?> GetFlightsByRouteAsync(int originAirportId, int destinationAirportId);
    }
}
