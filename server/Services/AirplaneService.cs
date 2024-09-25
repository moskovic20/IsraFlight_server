using Microsoft.EntityFrameworkCore;
using ServicesGatway.Models;


namespace ServicesGatway.Services
{
    public class AirplaneService
    {
        private readonly ApplicationDbContext _context;

        public AirplaneService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create
        public async Task CreateAirplane(Airplane airplane)
        {
            _context.Airplanes.Add(airplane);
            await _context.SaveChangesAsync();
        }

        // Read (all)
        public async Task<List<Airplane>> GetAllAirplanes()
        {
            return await _context.Airplanes.ToListAsync();
        }

        // Read (by ID)
        public async Task<Airplane> GetAirplaneById(int id)
        {
            return await _context.Airplanes.FindAsync(id);
        }

        // Update
        public async Task UpdateAirplane(Airplane airplane)
        {
            _context.Entry(airplane).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Delete
        public async Task DeleteAirplane(int id)
        {
            var airplane = await _context.Airplanes.FindAsync(id);
            if (airplane != null)
            {
                _context.Airplanes.Remove(airplane);
                await _context.SaveChangesAsync();
            }
        }
    }
}
