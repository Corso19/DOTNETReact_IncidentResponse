using IncidentResponseAPI.Dtos;
using IncidentResponseAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncidentResponseAPI.Services.Interfaces;

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
        public async Task<ActionResult<IEnumerable<EventsDto>>> GetAllEvents()
        {
            var events = await _eventsService.GetAllEventsAsync();
            return Ok(events);
        }
        
        // [HttpGet]
        // [SwaggerOperation(Summary = "Gets all events, ensuring sync on first load")]
        // public async Task<ActionResult<IEnumerable<EventsDto>>> GetEvents(string userId)
        // {
        //     // Ensure the database is synced with the inbox
        //     await _eventsService.SyncEventsAsync(userId);
        //
        //     // Fetch all events from the database
        //     var events = await _eventsService.GetAllEventsAsync();
        //     return Ok(events);
        // }

        // GET: api/Events/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets an event by ID")]
        public async Task<ActionResult<EventsDto>> GetEventbyId(int id)
        {
            var eventDto = await _eventsService.GetEventByIdAsync(id);
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
            await _eventsService.AddEventAsync(eventDto);
            return CreatedAtAction(nameof(GetEventbyId), new { id = eventDto.EventId }, eventDto);
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

            await _eventsService.UpdateEventAsync(eventDto);
            return NoContent();
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes an event by ID")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventDto = await _eventsService.GetEventByIdAsync(id);
            if (eventDto == null)
            {
                return NotFound();
            }

            await _eventsService.DeleteEventAsync(id);
            return NoContent();
        }
        
        // POST: api/Events/sync/{userId}
        [HttpPost("sync/{userId}")]
        [SwaggerOperation(Summary = "Syncs events (emails) for a specific user")]
        public async Task<IActionResult> SyncEvents(string userId)
        {
            await _eventsService.SyncEventsAsync(userId);
            return Ok("Events synced successfully.");
        }
        
        // GET: api/Events/{eventId}/attachments
        [HttpGet("{eventId}/attachments")]
        [SwaggerOperation(Summary = "Gets attachments for a specific event")]
        public async Task<ActionResult<IEnumerable<AttachmentDto>>> GetAttachmentsByEventId(int eventId)
        {
            var attachments = await _eventsService.GetAttachmentsByEventIdAsync(eventId);
            if (attachments == null || !attachments.Any())
            {
                return NotFound("No attachments found for the given event ID.");
            }
            return Ok(attachments);
        }
        
        // GET: api/Events/{userId}/message/{messageId}
        [HttpGet("{userId}/message/{messageId}")]
        [SwaggerOperation(Summary = "Fetches the content of a specific email message")]
        public async Task<ActionResult<string>> GetMessageContent(string userId, string messageId)
        {
            var message = await _eventsService.FetchMessageContentAsync(userId, messageId);
            if (message == null)
            {
                return NotFound("Message not found.");
            }
            return Ok(message.Body.Content);
        }
    }
}