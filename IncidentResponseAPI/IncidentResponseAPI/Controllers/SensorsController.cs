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
        private readonly ILogger<SensorsController> _logger;

        public SensorsController(ISensorsService sensorsService, ILogger<SensorsController> logger)
        {
            _sensorsService = sensorsService;
            _logger = logger;
        }

        // GET: api/Sensors
        [HttpGet]
        [SwaggerOperation(Summary = "Gets a list of sensors")]
        public async Task<ActionResult<IEnumerable<SensorsDto>>> GetAllSensors()
        {
            _logger.LogInformation("Fetching all sensors");

            try
            {
                var sensors = await _sensorsService.GetAllAsync();
                _logger.LogInformation("Successfully fetched {Count} sensors", sensors?.Count() ?? 0);
                return Ok(sensors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all sensors");
                return StatusCode(500, "An error occurred while fetching sensors.");
            }
        }

        // GET: api/Sensors/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets a sensor by ID")]
        public async Task<ActionResult<SensorsDto>> GetSensorById(int id)
        {
            _logger.LogInformation("Fetching sensor with ID {Id}", id);

            try
            {
                var sensor = await _sensorsService.GetByIdAsync(id);
                if (sensor == null)
                {
                    _logger.LogWarning("Sensor with ID {Id} not found", id);
                    return NotFound("Sensor not found.");
                }
                _logger.LogInformation("Successfully fetched sensor with ID {Id}", id);
                return Ok(sensor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching sensor with ID {Id}", id);
                return StatusCode(500, "An error occurred while fetching the sensor.");
            }
        }

        // POST: api/Sensors
        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new sensor")]
        public async Task<ActionResult<SensorsDto>> PostSensor(SensorsDto sensorsDto)
        {
            _logger.LogInformation("Creating a new sensor");

            try
            {
                await _sensorsService.AddAsync(sensorsDto);
                _logger.LogInformation("Successfully created sensor with ID {Id}", sensorsDto.SensorId);
                return CreatedAtAction(nameof(GetSensorById), new { id = sensorsDto.SensorId }, sensorsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new sensor");
                return StatusCode(500, "An error occurred while creating the sensor.");
            }
        }

        // PUT: api/Sensors/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Updates an existing sensor")]
        public async Task<IActionResult> PutSensor(int id, SensorsDto sensorsDto)
        {
            _logger.LogInformation("Updating sensor with ID {Id}", id);

            if (id != sensorsDto.SensorId)
            {
                _logger.LogWarning("Mismatched sensor ID in request body and URL: {Id}", id);
                return BadRequest("Sensor ID mismatch.");
            }

            try
            {
                await _sensorsService.UpdateAsync(id, sensorsDto);
                _logger.LogInformation("Successfully updated sensor with ID {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating sensor with ID {Id}", id);
                return StatusCode(500, "An error occurred while updating the sensor.");
            }
        }

        // DELETE: api/Sensors/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes a sensor by ID")]
        public async Task<IActionResult> DeleteSensor(int id)
        {
            _logger.LogInformation("Deleting sensor with ID {Id}", id);

            try
            {
                await _sensorsService.DeleteAsync(id);
                _logger.LogInformation("Successfully deleted sensor with ID {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting sensor with ID {Id}", id);
                return StatusCode(500, "An error occurred while deleting the sensor.");
            }
        }

        // PUT: api/Sensors/{id}/set-enabled
        [HttpPut("{id}/set-enabled")]
        [SwaggerOperation(Summary = "Sets a sensor's enabled status by ID")]
        public async Task<IActionResult> SetEnabled(int id, [FromQuery] bool isEnabled)
        {
            _logger.LogInformation("Setting enabled status for sensor with ID {Id} to {IsEnabled}", id, isEnabled);

            try
            {
                await _sensorsService.SetEnabledAsync(id, isEnabled);
                _logger.LogInformation("Successfully set enabled status for sensor with ID {Id} to {IsEnabled}", id, isEnabled);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while setting enabled status for sensor with ID {Id}", id);
                return StatusCode(500, "An error occurred while updating the sensor's enabled status.");
            }
        }
    }
}