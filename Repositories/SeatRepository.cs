using AtlasAir.Data;
using AtlasAir.Interfaces;
using AtlasAir.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var aircraftIds = await _context.FlightSegments
                .Where(fs => fs.FlightId == flightId)
                .Select(fs => fs.AircraftId)
                .Distinct()
                .ToListAsync();

            if (aircraftIds == null || !aircraftIds.Any())
            {
                return new List<Seat>();
            }

            var reservedSeatIds = await _context.Reservations
                .Where(r => r.FlightId == flightId && r.Status != AtlasAir.Enums.ReservationStatus.Cancelled)
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
