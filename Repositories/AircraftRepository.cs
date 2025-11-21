using AtlasAir.Data;
using AtlasAir.Interfaces;
using AtlasAir.Models;
using Microsoft.EntityFrameworkCore;

namespace AtlasAir.Repositories
{
    public class AircraftRepository(AtlasAirDbContext context) : IAircraftRepository
    {
        public async Task CreateAsync(Aircraft aircraft)
        {
            await context.Aircrafts.AddAsync(aircraft);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Aircraft aircraft)
        {
            context.Aircrafts.Remove(aircraft);
            await context.SaveChangesAsync();
        }

        public async Task<List<Aircraft>?> GetAllAsync()
        {
            return await context.Aircrafts.ToListAsync();
        }

        public async Task<Aircraft?> GetByIdAsync(int id)
        {
            return await context.Aircrafts.FindAsync(id);
        }

        public async Task UpdateAsync(Aircraft aircraft)
        {
            context.Aircrafts.Update(aircraft);
            await context.SaveChangesAsync();
        }
    }
}
