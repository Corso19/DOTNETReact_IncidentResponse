using IncidentResponseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace IncidentResponseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IncidentResponseContext _context;

        public EventsController(IncidentResponseContext context)
        {
            _context = context;
        }

        // GET: api/Events
        [HttpGet]
        [SwaggerOperation(Summary = "Gets a list of events")]
        public async Task<ActionResult<IEnumerable<EventsModel>>> GetEvents()
        {
            return await _context.Events.Include(e => e.Sensor).ToListAsync();
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets an event by ID")]
        public async Task<ActionResult<EventsModel>> GetEvent(int id)
        {
            var eventsModel = await _context.Events
                .Include(e => e.Sensor)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (eventsModel == null)
            {
                return NotFound();
            }

            return eventsModel;
        }

        // POST: api/Events
        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new event")]
        public async Task<ActionResult<EventsModel>> PostEvent(EventsModel eventsModel)
        {
            _context.Events.Add(eventsModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = eventsModel.EventId }, eventsModel);
        }

        // PUT: api/Events/5
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Updates an existing event")]
        public async Task<IActionResult> PutEvent(int id, EventsModel eventsModel)
        {
            if (id != eventsModel.EventId)
            {
                return BadRequest();
            }

            _context.Entry(eventsModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventsModelExists(id))
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

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes an event by ID")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventsModel = await _context.Events.FindAsync(id);
            if (eventsModel == null)
            {
                return NotFound();
            }

            _context.Events.Remove(eventsModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventsModelExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}


