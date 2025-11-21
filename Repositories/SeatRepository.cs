using AtlasAir.Data;
using AtlasAir.Interfaces;
using AtlasAir.Models;
using Microsoft.EntityFrameworkCore;

namespace AtlasAir.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly AtlasAirDbContext _context;

        public SeatRepository(AtlasAirDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Seat seat)
        {
            await _context.Seats.AddAsync(seat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Seat seat)
        {
            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Seat>?> GetAllAsync()
        {
            return await _context.Seats.ToListAsync();
        }

        public async Task<List<Seat>?> GetAvailableSeatsByFlightIdAsync(int flightId)
        {
            // 1) obter os AircraftId associados ao voo (via FlightSegment)
            var aircraftIds = await _context.FlightSegments
                .Where(fs => fs.FlightId == flightId)
                .Select(fs => fs.AircraftId)
                .Distinct()
                .ToListAsync();

            if (aircraftIds == null || !aircraftIds.Any())
            {
                // fallback: se não houver segmentos (voo sem segmento), tenta devolver assentos do primeiro aircraft relacionado ao voo
                var maybeAircraftId = await _context.FlightSegments
                    .Where(fs => fs.FlightId == flightId)
                    .Select(fs => (int?)fs.AircraftId)
                    .FirstOrDefaultAsync();

                if (maybeAircraftId.HasValue)
                    aircraftIds = new List<int> { maybeAircraftId.Value };
                else
                    return new List<Seat>();
            }

            // 2) buscar assentos desses aircrafts que NÃO estejam reservados para esse voo
            var reservedSeatIds = await _context.Reservations
                .Where(r => r.FlightId == flightId)
                .Select(r => r.SeatId)
                .ToListAsync();

            var seats = await _context.Seats
                .Where(s => aircraftIds.Contains(s.AircraftId) && !reservedSeatIds.Contains(s.Id))
                .ToListAsync();

            return seats;
        }

        public async Task<Seat?> GetByIdAsync(int id)
        {
            return await _context.Seats.FindAsync(id);
        }

        public async Task UpdateAsync(Seat seat)
        {
            _context.Seats.Update(seat);
            await _context.SaveChangesAsync();
        }
    }
}
