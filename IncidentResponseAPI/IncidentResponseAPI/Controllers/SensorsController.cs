using IncidentResponseAPI.Dtos;
using IncidentResponseAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IncidentResponseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorsController : ControllerBase
    {
        private readonly ISensorsService _sensorsService;

        public SensorsController(ISensorsService sensorsService)
        {
            _sensorsService = sensorsService;
        }

        // GET: api/Sensors
        [HttpGet]
        [SwaggerOperation(Summary = "Gets a list of sensors")]
        public async Task<ActionResult<IEnumerable<SensorsDto>>> GetSensors()
        {
            return Ok(await _sensorsService.GetAllAsync());
        }

        // GET: api/Sensors/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets a sensor by ID")]
        public async Task<ActionResult<SensorsDto>> GetSensor(int id)
        {
            var sensorDto = await _sensorsService.GetByIdAsync(id);

            if (sensorDto == null)
            {
                return NotFound();
            }

            return sensorDto;
        }

        // POST: api/Sensors
        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new sensor")]
        public async Task<ActionResult<SensorsDto>> PostSensor(SensorsDto sensorsDto)
        {
            await _sensorsService.AddAsync(sensorsDto);
            return CreatedAtAction(nameof(GetSensor), new { id = sensorsDto.SensorId }, sensorsDto);
        }

        // PUT: api/Sensors/5
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Updates an existing sensor")]
        public async Task<IActionResult> PutSensor(int id, SensorsDto sensorsDto)
        {
            await _sensorsService.UpdateAsync(id, sensorsDto);
            return NoContent();
        }

        // DELETE: api/Sensors/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes a sensor by ID")]
        public async Task<IActionResult> DeleteSensor(int id)
        {
            await _sensorsService.DeleteAsync(id);
            return NoContent();
        }
        
        //PUT: api/Sensors/5/setEnabled
        [HttpPut("{id}/set-enabled")]
        [SwaggerOperation(Summary = "Sets a sensor's enabled status by ID")]
        public async Task<IActionResult> SetEnabled(int id, bool isEnabled)
        {
            await _sensorsService.SetEnabledAsync(id, isEnabled);
            return NoContent();
        }
    }
}