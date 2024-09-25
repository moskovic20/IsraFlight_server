using Microsoft.AspNetCore.Mvc;
using ServicesGatway.Services;
using ServicesGatway.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ServicesGatway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly FlightService _flightService;
        private readonly ShabbatTimeService _shabbatTimeService;

        public FlightController(FlightService flightService, ShabbatTimeService shabbatTimeService)
        {
            _flightService = flightService;
            _shabbatTimeService = shabbatTimeService;
        }

        // פונקציית עזר פרטית לבדוק נחיתות בשבת
        private async Task LandingCheck(DateTime arrivalDate, string arrivalCity)
        {
            var shabbatTimes = await _shabbatTimeService.GetShabbatTimesAsync(arrivalDate, arrivalCity);

            if (shabbatTimes.Entry == null || shabbatTimes.Exit == null)
            {
                throw new Exception("Failed to retrieve Shabbat times.");
            }

            if (arrivalDate >= shabbatTimes.Entry && arrivalDate <= shabbatTimes.Exit)
            {
                throw new Exception("Landing time during Shabbat is not allowed.");
            }
        }

        // GET: api/Flights
        [HttpGet]
        public async Task<ActionResult<IQueryable<Flight>>> Get()
        {
            var flights = await _flightService.GetAllFlights();
            return Ok(flights);
        }

        // GET: api/Flights/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlight(int id)
        {
            var flight = await _flightService.GetFlightById(id);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }

        // POST: api/Flights
        [HttpPost]
        public async Task<ActionResult<Flight>> PostFlight(Flight flight)
        {
            try
            {
                await LandingCheck(flight.ArrivalDateTime, flight.ArrivalCity);
                await _flightService.CreateFlight(flight);
                return CreatedAtAction("GetFlight", new { id = flight.FlightId }, flight);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating the flight: {ex.Message}");
            }
        }

        // PUT: api/Flights/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlight(int id, Flight flight)
        {
            if (id != flight.FlightId)
            {
                return BadRequest("Error: There is no such ID in our system.");
            }

            try
            {
                await LandingCheck(flight.ArrivalDateTime, flight.ArrivalCity);
                await _flightService.UpdateFlight(flight);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating the flight: {ex.Message}");
            }
        }

        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            await _flightService.DeleteFlight(id);
            return NoContent();
        }

        // פונקציות חיפוש מעודכנות
        [HttpGet("by-origin")]
        public async Task<ActionResult<List<Flight>>> ByOrigin(string city, string country)
        {
            try
            {
                var flights = await _flightService.ByOrigin(city, country);
                if (flights == null || flights.Count == 0)
                {
                    return NotFound("No matching flights found.");
                }
                return Ok(flights);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving flights: {ex.Message}");
            }
        }

        [HttpGet("by-destination")]
        public async Task<ActionResult<List<Flight>>> ByDestination(string city, string country)
        {
            try
            {
                var flights = await _flightService.ByDestination(city, country);
                if (flights == null || flights.Count == 0)
                {
                    return NotFound("No matching flights found.");
                }
                return Ok(flights);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving flights: {ex.Message}");
            }
        }

        [HttpGet("by-origin-destination")]
        public async Task<ActionResult<List<Flight>>> ByOriginAndDestination(string originCity, string originCountry, string destinationCity, string destinationCountry)
        {
            try
            {
                var flights = await _flightService.ByOriginAndDestination(originCity, originCountry, destinationCity, destinationCountry);
                if (flights == null || flights.Count == 0)
                {
                    return NotFound("No matching flights found.");
                }
                return Ok(flights);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving flights: {ex.Message}");
            }
        }

        // GET: api/Flights/available-flights
        [HttpGet("available-flights")]
        public async Task<ActionResult<List<Flight>>> GetAvailableFlights()
        {
            try
            {
                var flights = await _flightService.GetUpcoming();
                if (flights == null || flights.Count == 0)
                {
                    return NotFound("No flights found.");
                }
                return Ok(flights);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


    }
}
