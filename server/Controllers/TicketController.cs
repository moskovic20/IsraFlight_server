using Microsoft.AspNetCore.Mvc;
using ServicesGatway.Services;
using ServicesGatway.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ServicesGatway.Controllers
{
    [Route("api/[controller]")] // שיניתי את הנתיב ל-api במקום odata
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketService _ticketService;

        public TicketController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IQueryable<Ticket>>> Get()
        {
            var tickets = await _ticketService.GetAllTickets();
            return Ok(tickets);
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _ticketService.GetTicketById(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(ticket);
        }

        // POST: api/Tickets
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
            await _ticketService.CreateTicket(ticket);
            return CreatedAtAction("GetTicket", new { id = ticket.TicketId }, ticket);
        }

        // PUT: api/Tickets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return BadRequest();
            }

            await _ticketService.UpdateTicket(ticket);
            return NoContent();
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            await _ticketService.DeleteTicket(id);
            return NoContent();
        }
    }
}
