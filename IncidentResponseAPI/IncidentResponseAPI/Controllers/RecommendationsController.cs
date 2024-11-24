using IncidentResponseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IncidentResponseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationsController : ControllerBase
    {
        private readonly IncidentResponseContext _context;

        public RecommendationsController(IncidentResponseContext context)
        {
            _context = context;
        }

        // GET: api/Recommendations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecommendationsModel>>> GetRecommendations()
        {
            return await _context.RecommendationsModel.Include(r => r.Incident).ToListAsync();
        }

        // GET: api/Recommendations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RecommendationsModel>> GetRecommendation(int id)
        {
            var recommendationsModel = await _context.RecommendationsModel
                .Include(r => r.Incident)
                .FirstOrDefaultAsync(m => m.RecommendationId == id);

            if (recommendationsModel == null)
            {
                return NotFound();
            }

            return recommendationsModel;
        }

        // POST: api/Recommendations
        [HttpPost]
        public async Task<ActionResult<RecommendationsModel>> PostRecommendation(RecommendationsModel recommendationsModel)
        {
            _context.RecommendationsModel.Add(recommendationsModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRecommendation), new { id = recommendationsModel.RecommendationId }, recommendationsModel);
        }

        // PUT: api/Recommendations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecommendation(int id, RecommendationsModel recommendationsModel)
        {
            if (id != recommendationsModel.RecommendationId)
            {
                return BadRequest();
            }

            _context.Entry(recommendationsModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecommendationsModelExists(id))
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

        // DELETE: api/Recommendations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecommendation(int id)
        {
            var recommendationsModel = await _context.RecommendationsModel.FindAsync(id);
            if (recommendationsModel == null)
            {
                return NotFound();
            }

            _context.RecommendationsModel.Remove(recommendationsModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecommendationsModelExists(int id)
        {
            return _context.RecommendationsModel.Any(e => e.RecommendationId == id);
        }
    }
}

