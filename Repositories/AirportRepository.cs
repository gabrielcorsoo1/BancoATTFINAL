using AtlasAir.Data;
using AtlasAir.Interfaces;
using AtlasAir.Models;
using Microsoft.EntityFrameworkCore;

namespace AtlasAir.Repositories
{
    public class AirportRepository(AtlasAirDbContext context) : IAirportRepository
    {
        public async Task CreateAsync(Airport airport)
        {
            await context.Airports.AddAsync(airport);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Airport airport)
        {
            context.Airports.Remove(airport);
            await context.SaveChangesAsync();
        }

        public async Task<List<Airport>?> GetAllAsync()
        {
            return await context.Airports.ToListAsync();
        }

        public async Task<Airport?> GetByIdAsync(int id)
        {
            return await context.Airports.FindAsync(id);
        }

        public async Task UpdateAsync(Airport airport)
        {
            context.Airports.Update(airport);
            await context.SaveChangesAsync();
        }
    }
}
