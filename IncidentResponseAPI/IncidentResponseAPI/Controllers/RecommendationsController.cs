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
    public class RecommendationsController : Controller
    {
        private readonly IncidentResponseContext _context;

        public RecommendationsController(IncidentResponseContext context)
        {
            _context = context;
        }

        // GET: Recommendations
        public async Task<IActionResult> Index()
        {
            var incidentResponseContext = _context.RecommendationsModel.Include(r => r.Incident);
            return View(await incidentResponseContext.ToListAsync());
        }

        // GET: Recommendations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recommendationsModel = await _context.RecommendationsModel
                .Include(r => r.Incident)
                .FirstOrDefaultAsync(m => m.RecommendationId == id);
            if (recommendationsModel == null)
            {
                return NotFound();
            }

            return View(recommendationsModel);
        }

        // GET: Recommendations/Create
        public IActionResult Create()
        {
            ViewData["IncidentId"] = new SelectList(_context.Incidents, "IncidentId", "IncidentId");
            return View();
        }

        // POST: Recommendations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecommendationId,IncidentId,Recommendation,isCompleted")] RecommendationsModel recommendationsModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recommendationsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IncidentId"] = new SelectList(_context.Incidents, "IncidentId", "IncidentId", recommendationsModel.IncidentId);
            return View(recommendationsModel);
        }

        // GET: Recommendations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recommendationsModel = await _context.RecommendationsModel.FindAsync(id);
            if (recommendationsModel == null)
            {
                return NotFound();
            }
            ViewData["IncidentId"] = new SelectList(_context.Incidents, "IncidentId", "IncidentId", recommendationsModel.IncidentId);
            return View(recommendationsModel);
        }

        // POST: Recommendations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecommendationId,IncidentId,Recommendation,isCompleted")] RecommendationsModel recommendationsModel)
        {
            if (id != recommendationsModel.RecommendationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recommendationsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecommendationsModelExists(recommendationsModel.RecommendationId))
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
            ViewData["IncidentId"] = new SelectList(_context.Incidents, "IncidentId", "IncidentId", recommendationsModel.IncidentId);
            return View(recommendationsModel);
        }

        // GET: Recommendations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recommendationsModel = await _context.RecommendationsModel
                .Include(r => r.Incident)
                .FirstOrDefaultAsync(m => m.RecommendationId == id);
            if (recommendationsModel == null)
            {
                return NotFound();
            }

            return View(recommendationsModel);
        }

        // POST: Recommendations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recommendationsModel = await _context.RecommendationsModel.FindAsync(id);
            if (recommendationsModel != null)
            {
                _context.RecommendationsModel.Remove(recommendationsModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecommendationsModelExists(int id)
        {
            return _context.RecommendationsModel.Any(e => e.RecommendationId == id);
        }
    }
}
