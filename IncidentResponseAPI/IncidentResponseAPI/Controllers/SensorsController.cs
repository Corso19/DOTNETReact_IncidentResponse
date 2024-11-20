using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IncidentResponseAPI.Models;

namespace IncidentResponseAPI.Controllers
{
    public class SensorsController : Controller
    {
        private readonly IncidentResponseContext _context;

        public SensorsController(IncidentResponseContext context)
        {
            _context = context;
        }

        // GET: Sensors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sensors.ToListAsync());
        }

        // GET: Sensors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sensorsModel = await _context.Sensors
                .FirstOrDefaultAsync(m => m.SensorId == id);
            if (sensorsModel == null)
            {
                return NotFound();
            }

            return View(sensorsModel);
        }

        // GET: Sensors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sensors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SensorId,SensorName,Type,ConfigurationJson,isEnabled,CreatedAd,LastRunAt")] SensorsModel sensorsModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sensorsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sensorsModel);
        }

        // GET: Sensors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sensorsModel = await _context.Sensors.FindAsync(id);
            if (sensorsModel == null)
            {
                return NotFound();
            }
            return View(sensorsModel);
        }

        // POST: Sensors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SensorId,SensorName,Type,ConfigurationJson,isEnabled,CreatedAd,LastRunAt")] SensorsModel sensorsModel)
        {
            if (id != sensorsModel.SensorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sensorsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SensorsModelExists(sensorsModel.SensorId))
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
            return View(sensorsModel);
        }

        // GET: Sensors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sensorsModel = await _context.Sensors
                .FirstOrDefaultAsync(m => m.SensorId == id);
            if (sensorsModel == null)
            {
                return NotFound();
            }

            return View(sensorsModel);
        }

        // POST: Sensors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sensorsModel = await _context.Sensors.FindAsync(id);
            if (sensorsModel != null)
            {
                _context.Sensors.Remove(sensorsModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SensorsModelExists(int id)
        {
            return _context.Sensors.Any(e => e.SensorId == id);
        }
    }
}
