using AtlasAir.Data;
using AtlasAir.Interfaces;
using AtlasAir.Models;
using Microsoft.EntityFrameworkCore;

namespace AtlasAir.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly AtlasAirDbContext _context;

        public FlightRepository(AtlasAirDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Flight flight)
        {
            await _context.Flights.AddAsync(flight);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Flight flight)
        {
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Flight>?> GetAllAsync()
        {
            return await _context.Flights
                .Include(f => f.OriginAirport)
                .Include(f => f.DestinationAirport)
                .Include(f => f.FlightSegments)
                    .ThenInclude(fs => fs.OriginAirport)
                .Include(f => f.FlightSegments)
                    .ThenInclude(fs => fs.DestinationAirport)
                .ToListAsync();
        }

        public async Task<Flight?> GetByIdAsync(int id)
        {
            return await _context.Flights
                .Include(f => f.OriginAirport)
                .Include(f => f.DestinationAirport)
                .Include(f => f.FlightSegments)
                    .ThenInclude(fs => fs.OriginAirport)
                .Include(f => f.FlightSegments)
                    .ThenInclude(fs => fs.DestinationAirport)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<List<Flight>?> GetFlightsByRouteAsync(int originAirportId, int destinationAirportId)
        {
            return await _context.Flights
                .Where(f => f.OriginAirportId == originAirportId && f.DestinationAirportId == destinationAirportId)
                .ToListAsync();
        }

        public async Task UpdateAsync(Flight flight)
        {
            _context.Flights.Update(flight);
            await _context.SaveChangesAsync();
        }
    }
}
