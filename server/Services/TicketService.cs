using Microsoft.EntityFrameworkCore;
using ServicesGatway.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesGatway.Services
{
    public class TicketService
    {
        private readonly ApplicationDbContext _context;

        public TicketService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create
        public async Task CreateTicket(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
        }

        // Read (all)
        public async Task<List<Ticket>> GetAllTickets()
        {
            return await _context.Tickets.ToListAsync();
        }

        // Read (by ID)
        public async Task<Ticket> GetTicketById(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        // Update
        public async Task UpdateTicket(Ticket ticket)
        {
            _context.Entry(ticket).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Delete
        public async Task DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

        // TicketService.cs

        public async Task<List<Ticket>> GetTicketsByCustomerId(int customerId)
        {
            return await _context.Tickets
                .Where(t => t.CustomerId == customerId)
                .ToListAsync();
        }

    }
}
