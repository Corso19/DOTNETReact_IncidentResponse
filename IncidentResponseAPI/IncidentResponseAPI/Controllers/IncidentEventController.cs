using IncidentResponseAPI.Dtos;
using IncidentResponseAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IncidentResponseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentEventController : ControllerBase
    {
        private readonly IIncidentEventService _incidentEventService;

        public IncidentEventController(IIncidentEventService incidentEventService)
        {
            _incidentEventService = incidentEventService;
        }

        // GET: api/IncidentEvent
        [HttpGet]
        [SwaggerOperation(Summary = "Gets a list of incident events")]
        public async Task<ActionResult<IEnumerable<IncidentEventDto>>> GetIncidentEvents()
        {
            return Ok(await _incidentEventService.GetAllAsync());
        }

        // GET: api/IncidentEvent/{incidentId}/{eventId}
        [HttpGet("{incidentId}/{eventId}")]
        [SwaggerOperation(Summary = "Gets an incident event by incident ID and event ID")]
        public async Task<ActionResult<IncidentEventDto>> GetIncidentEvent(int incidentId, int eventId)
        {
            var incidentEventDto = await _incidentEventService.GetByIdAsync(incidentId, eventId);

            if (incidentEventDto == null)
            {
                return NotFound();
            }

            return incidentEventDto;
        }

        // POST: api/IncidentEvent
        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new incident event")]
        public async Task<ActionResult<IncidentEventDto>> PostIncidentEvent(IncidentEventDto incidentEventDto)
        {
            await _incidentEventService.AddAsync(incidentEventDto);
            return CreatedAtAction(nameof(GetIncidentEvent), new { incidentId = incidentEventDto.IncidentId, eventId = incidentEventDto.EventId }, incidentEventDto);
        }

        // DELETE: api/IncidentEvent/{incidentId}/{eventId}
        [HttpDelete("{incidentId}/{eventId}")]
        [SwaggerOperation(Summary = "Deletes an incident event by incident ID and event ID")]
        public async Task<IActionResult> DeleteIncidentEvent(int incidentId, int eventId)
        {
            await _incidentEventService.DeleteAsync(incidentId, eventId);
            return NoContent();
        }
    }
}




