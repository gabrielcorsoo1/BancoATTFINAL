using AtlasAir.Data;
using AtlasAir.Interfaces;
using AtlasAir.Models;
using Microsoft.EntityFrameworkCore;

namespace AtlasAir.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AtlasAirDbContext _context;

        public ReservationRepository(AtlasAirDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Reservation>?> GetAllAsync()
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Flight)
                    .ThenInclude(f => f.OriginAirport)
                .Include(r => r.Flight)
                    .ThenInclude(f => f.DestinationAirport)
                .Include(r => r.Seat)
                .ToListAsync();
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.Customer)
                .Include(r => r.Flight)
                    .ThenInclude(f => f.OriginAirport)
                .Include(r => r.Flight)
                    .ThenInclude(f => f.DestinationAirport)
                .Include(r => r.Seat)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
        }
    }
}
