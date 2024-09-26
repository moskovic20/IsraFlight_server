using Microsoft.AspNetCore.Mvc;
using ServicesGatway.Services;
using ServicesGatway.Models;
using System.Threading.Tasks;

namespace ServicesGatway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirplaneController : ControllerBase
    {
        private readonly AirplaneService _airplaneService;
        private readonly ImaggaService _imaggaService;

        public AirplaneController(AirplaneService airplaneService, ImaggaService imaggaService)
        {
            _airplaneService = airplaneService;
            _imaggaService = imaggaService;
        }

        // GET: api/Airplanes
        [HttpGet]
        public async Task<IActionResult> GetAllAirplanes()
        {
            var airplanes = await _airplaneService.GetAllAirplanes();
            return Ok(airplanes);
        }

        // GET: api/Airplanes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Airplane>> GetAirplaneById(int id)
        {
            var airplane = await _airplaneService.GetAirplaneById(id);
            if (airplane == null)
            {
                return NotFound();
            }
            return Ok(airplane);
        }

        // POST: api/Airplanes
        [HttpPost]
        public async Task<IActionResult> CreateAirplane([FromBody] Airplane airplane)
        {
            // קריאה לשירות Imagga כדי לבדוק אם התמונה היא של מטוס
            var isAirplaneImage = await _imaggaService.IsAirplane(airplane.ImageUrl);

            if (!isAirplaneImage)
            {
                return BadRequest("The image is not airplane.");
            }

            // אם התמונה זוהתה כמטוס, נוסיף את המטוס למסד הנתונים
            await _airplaneService.CreateAirplane(airplane);
            return CreatedAtAction(nameof(GetAirplaneById), new { id = airplane.AirplaneId }, airplane);
        }

        // PUT: api/Airplanes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAirplane(int id, [FromBody] Airplane airplane)
        {
            if (id != airplane.AirplaneId)
            {
                return BadRequest("Airplane ID mismatch.");
            }

            await _airplaneService.UpdateAirplane(airplane);
            return NoContent();
        }

        // DELETE: api/Airplanes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAirplane(int id)
        {
            var airplane = await _airplaneService.GetAirplaneById(id);
            if (airplane == null)
            {
                return NotFound();
            }

            await _airplaneService.DeleteAirplane(id);
            return NoContent();
        }
    }
}
