using Microsoft.EntityFrameworkCore;
using ServicesGatway.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesGatway.Services
{
    public class FlightService
    {
        private readonly ApplicationDbContext _context;

        public FlightService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create
        public async Task CreateFlight(Flight flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
        }

        // Read (all)
        public async Task<List<Flight>> GetAllFlights()
        {
            return await _context.Flights.ToListAsync();
        }

        // Read (by ID)
        public async Task<Flight?> GetFlightById(int id)
        {
            return await _context.Flights.FindAsync(id);
        }

        // Update
        public async Task UpdateFlight(Flight flight)
        {
            _context.Entry(flight).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Delete
        public async Task DeleteFlight(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight != null)
            {
                _context.Flights.Remove(flight);
                await _context.SaveChangesAsync();
            }
        }

        // פונקציות חדשות בהתאם
        public async Task<List<Flight>> ByOrigin(string city, string country)
        {
            return await _context.Flights
                .Where(f => f.DepartureCity == city && f.DepartureCountry == country)
                .Where(f => f.DepartureDateTime >= DateTime.Now.AddDays(1) && f.AvailableSeats > 0)
                .ToListAsync();
        }

        public async Task<List<Flight>> ByDestination(string city, string country)
        {
            return await _context.Flights
                .Where(f => f.ArrivalCity == city && f.ArrivalCountry == country)
                .Where(f => f.DepartureDateTime >= DateTime.Now.AddDays(1) && f.AvailableSeats > 0)
                .ToListAsync();
        }

        public async Task<List<Flight>> ByOriginAndDestination(string originCity, string originCountry, string destinationCity, string destinationCountry)
        {
            return await _context.Flights
                .Where(f => f.DepartureCity == originCity && f.DepartureCountry == originCountry)
                .Where(f => f.ArrivalCity == destinationCity && f.ArrivalCountry == destinationCountry)
                .Where(f => f.DepartureDateTime >= DateTime.Now.AddDays(1) && f.AvailableSeats > 0)
                .ToListAsync();
        }

        public async Task<List<Flight>> GetUpcoming()
        {
            return await _context.Flights
                .Where(f => f.DepartureDateTime >= DateTime.Now.AddDays(1) && f.AvailableSeats > 0)
                .ToListAsync();
        }

    }
}
