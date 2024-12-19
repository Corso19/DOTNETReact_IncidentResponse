using IncidentResponseAPI.Dtos;
using IncidentResponseAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using IncidentResponseAPI.Services.Interfaces;

namespace IncidentResponseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventsService;
        private readonly ILogger<EventsController> _logger;

        public EventsController(IEventsService eventsService, ILogger<EventsController> logger)
        {
            _eventsService = eventsService;
            _logger = logger;
        }

        // GET: api/Events
        [HttpGet]
        [SwaggerOperation(Summary = "Gets a list of events")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAllEvents()
        {
            _logger.LogInformation("Fetching all events");

            try
            {
                var events = await _eventsService.GetAllEventsAsync();
                _logger.LogInformation("Successfuly fetched {Count} events", events?.Count() ?? 0);
                return Ok(events);
            }
            catch (Exception ex)
            { 
                _logger.LogError(ex, "Error occured while fetching all events.");
                return StatusCode(500, "An error occured while fetching events.");
            }
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets an event by ID")]
        public async Task<ActionResult<EventDto>> GetEventbyId(int id)
        {
            _logger.LogInformation("Fetching event with ID {Id}", id);

            try
            {
                var eventDto = await _eventsService.GetEventByIdAsync(id);
                if (eventDto == null)
                {
                    _logger.LogWarning("Event with ID {EventId} not found", id);
                    return NotFound($"Event with ID {id} not found.");
                }
                _logger.LogInformation("Successfully fetched event with ID {Id}", id);
                return Ok(eventDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while fetching event with ID {Id}", id);
                return StatusCode(500, "An error occured while fetching the event.");
            }
        }

        // POST: api/Events
        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new event")]
        public async Task<ActionResult<EventDto>> PostEvent(EventDto eventDto)
        {
            _logger.LogInformation("Adding a new event");

            try
            {
                await _eventsService.AddEventAsync(eventDto);
                _logger.LogInformation("Successfully added a new event with ID {Id}", eventDto.EventId);
                return CreatedAtAction(nameof(GetEventbyId), new { id = eventDto.EventId }, eventDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while adding a new event.");
                return StatusCode(500, "An error occured while adding the event.");
            }
        }

        // PUT: api/Events/5
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Updates an existing event")]
        public async Task<IActionResult> PutEvent(int id, EventDto eventDto)
        {
            _logger.LogInformation("Updating event with ID {Id}", id);
            
            if (id != eventDto.EventId)
            {
                _logger.LogWarning("Event ID in the ({RequesstId}) does not match the ({PayloadId})", id, eventDto.EventId);
                return BadRequest("Event ID mismatch");
            }

            try
            {
                await _eventsService.UpdateEventAsync(eventDto);
                _logger.LogInformation("Successfully updated event with ID {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating event with ID {Id}", id);
                return StatusCode(500, "An error occured while updating the event.");
            }
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes an event by ID")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            _logger.LogInformation("Deleting event with ID {EventId}", id);

            try
            {
                var eventDto = await _eventsService.GetEventByIdAsync(id);
                if (eventDto == null)
                {
                    _logger.LogWarning("Event with ID {EventId} not found", id);
                    return NotFound($"Event with ID {id} not found.");
                }

                await _eventsService.DeleteEventAsync(id);
                _logger.LogInformation("Successfully deleted event with ID {EventId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting event with ID {EventId}", id);
                return StatusCode(500, "An error occured while deleting the event.");
            }
        }
        
        // POST: api/Events/sync/{userId}
        [HttpPost("sync/{userId}")]
        [SwaggerOperation(Summary = "Syncs events (emails) for a specific user")]
        public async Task<IActionResult> SyncEvents(string userId)
        {
            _logger.LogInformation("Syncing events for user {UserId}", userId);

            try
            {
                await _eventsService.SyncEventsAsync(userId);
                _logger.LogInformation("Events synced successfully for user {UserId}", userId);
                return Ok("Events synced successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while syncing events for user {UserId}", userId);
                return StatusCode(500, "An error occured while syncing events.");
            }
        }
        
        // GET: api/Events/{eventId}/attachments
        [HttpGet("{eventId}/attachments")]
        [SwaggerOperation(Summary = "Gets attachments for a specific event")]
        public async Task<ActionResult<IEnumerable<AttachmentDto>>> GetAttachmentsByEventId(int eventId)
        {
            _logger.LogInformation("Fetching attachments for event with ID {EventId}", eventId);

            try
            {
                var attachments = await _eventsService.GetAttachmentsByEventIdAsync(eventId);
                if (attachments == null || !attachments.Any())
                {
                    _logger.LogWarning("No attachments found for event with ID {EventId}", eventId);
                    return NotFound("No attachments found for the given event ID.");
                }
                _logger.LogInformation("Successfully fetched {Count} attachments for event with ID {EventId}", attachments.Count(), eventId);
                return Ok(attachments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while fetching attachments for event with ID {EventId}", eventId);
                return StatusCode(500, "An error occured while fetching attachments.");
            }
        }
        
        // GET: api/Events/{userId}/message/{messageId}
        [HttpGet("{userId}/message/{messageId}")]
        [SwaggerOperation(Summary = "Fetches the content of a specific email message")]
        public async Task<ActionResult<string>> GetMessageContent(string userId, string messageId)
        {
            _logger.LogInformation("Fetching content for message with ID {MessageId} for user {UserId}", messageId, userId);

            try
            {
                var message = await _eventsService.FetchMessageContentAsync(userId, messageId);
                if (message == null)
                {
                    _logger.LogWarning("Message with ID {MessageId} not found for user {UserId}", messageId, userId);
                    return NotFound("Message not found.");
                }
                _logger.LogInformation("Successfully fetched content for message with ID {MessageId} for user {UserId}", messageId, userId);
                return Ok(message.Body.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while fetching content for message with ID {MessageId} for user {UserId}", messageId, userId);
                return StatusCode(500, "An error occured while fetching message content.");
            }
        }
    }
}