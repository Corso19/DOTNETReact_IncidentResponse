using IncidentResponseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IncidentResponseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly IncidentResponseContext _context;

        public EventsController(IncidentResponseContext context)
        {
            _context = context;
        }

        // GET: Events
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var incidentResponseContext = _context.Events.Include(e => e.Sensor);
            return View(await incidentResponseContext.ToListAsync());
        }

        // GET: Events/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventsModel = await _context.Events
                .Include(e => e.Sensor)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (eventsModel == null)
            {
                return NotFound();
            }

            return View(eventsModel);
        }

        // GET: Events/Create
        [HttpPost]
        public IActionResult Create()
        {
            ViewData["SensorId"] = new SelectList(_context.Sensors, "SensorId", "SensorId");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,SensorId,EventDataJson,Timestamp,isProcessed")] EventsModel eventsModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SensorId"] = new SelectList(_context.Sensors, "SensorId", "SensorId", eventsModel.SensorId);
            return View(eventsModel);
        }

        // GET: Events/Edit/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventsModel = await _context.Events.FindAsync(id);
            if (eventsModel == null)
            {
                return NotFound();
            }
            ViewData["SensorId"] = new SelectList(_context.Sensors, "SensorId", "SensorId", eventsModel.SensorId);
            return View(eventsModel);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,SensorId,EventDataJson,Timestamp,isProcessed")] EventsModel eventsModel)
        {
            if (id != eventsModel.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventsModelExists(eventsModel.EventId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SensorId"] = new SelectList(_context.Sensors, "SensorId", "SensorId", eventsModel.SensorId);
            return View(eventsModel);
        }

        // GET: Events/Delete/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventsModel = await _context.Events
                .Include(e => e.Sensor)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (eventsModel == null)
            {
                return NotFound();
            }

            return View(eventsModel);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventsModel = await _context.Events.FindAsync(id);
            if (eventsModel != null)
            {
                _context.Events.Remove(eventsModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventsModelExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
