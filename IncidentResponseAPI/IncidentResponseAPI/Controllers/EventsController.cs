using IncidentResponseAPI.Dtos;
using IncidentResponseAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IncidentResponseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventsService;

        public EventsController(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }

        // GET: api/Events
        [HttpGet]
        [SwaggerOperation(Summary = "Gets a list of events")]
        public async Task<ActionResult<IEnumerable<EventsDto>>> GetEvents()
        {
            var events = await _eventsService.GetAllAsync();
            return Ok(events);
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets an event by ID")]
        public async Task<ActionResult<EventsDto>> GetEvent(int id)
        {
            var eventDto = await _eventsService.GetByIdAsync(id);
            if (eventDto == null)
            {
                return NotFound();
            }
            return Ok(eventDto);
        }

        // POST: api/Events
        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new event")]
        public async Task<ActionResult<EventsDto>> PostEvent(EventsDto eventDto)
        {
            await _eventsService.AddAsync(eventDto);
            return CreatedAtAction(nameof(GetEvent), new { id = eventDto.EventId }, eventDto);
        }

        // PUT: api/Events/5
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Updates an existing event")]
        public async Task<IActionResult> PutEvent(int id, EventsDto eventDto)
        {
            if (id != eventDto.EventId)
            {
                return BadRequest();
            }

            await _eventsService.UpdateAsync(id, eventDto);
            return NoContent();
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes an event by ID")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventDto = await _eventsService.GetByIdAsync(id);
            if (eventDto == null)
            {
                return NotFound();
            }

            await _eventsService.DeleteAsync(id);
            return NoContent();
        }
    }
}