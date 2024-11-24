using IncidentResponseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace IncidentResponseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorsController : ControllerBase
    {
        private readonly IncidentResponseContext _context;

        public SensorsController(IncidentResponseContext context)
        {
            _context = context;
        }

        // GET: api/Sensors
        [HttpGet]
        [SwaggerOperation(Summary = "Gets a list of sensors")]
        public async Task<ActionResult<IEnumerable<SensorsModel>>> GetSensors()
        {
            return await _context.Sensors.ToListAsync();
        }

        // GET: api/Sensors/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets a sensor by ID")]
        public async Task<ActionResult<SensorsModel>> GetSensor(int id)
        {
            var sensorsModel = await _context.Sensors.FindAsync(id);

            if (sensorsModel == null)
            {
                return NotFound();
            }

            return sensorsModel;
        }

        // POST: api/Sensors
        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new sensor")]
        public async Task<ActionResult<SensorsModel>> PostSensor(SensorsModel sensorsModel)
        {
            _context.Sensors.Add(sensorsModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSensor), new { id = sensorsModel.SensorId }, sensorsModel);
        }

        // PUT: api/Sensors/5
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Updates an existing sensor")]
        public async Task<IActionResult> PutSensor(int id, SensorsModel sensorsModel)
        {
            if (id != sensorsModel.SensorId)
            {
                return BadRequest();
            }

            _context.Entry(sensorsModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensorsModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Sensors/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes a sensor by ID")]
        public async Task<IActionResult> DeleteSensor(int id)
        {
            var sensorsModel = await _context.Sensors.FindAsync(id);
            if (sensorsModel == null)
            {
                return NotFound();
            }

            _context.Sensors.Remove(sensorsModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SensorsModelExists(int id)
        {
            return _context.Sensors.Any(e => e.SensorId == id);
        }
    }
}
