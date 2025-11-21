using AtlasAir.Models;

namespace AtlasAir.Interfaces
{
    public interface IFlightSegmentRepository
    {
        Task CreateAsync(FlightSegment flightSegment);
        Task<FlightSegment?> GetByIdAsync(int id);
        Task<List<FlightSegment>?> GetAllAsync();
        Task UpdateAsync(FlightSegment flightSegment);
        Task DeleteAsync(FlightSegment flightSegment);
    }
}
