using IncidentResponseAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IncidentResponseAPI.Repositories
{
    public class IncidentEventRepository : IIncidentEventRepository
    {
        private readonly IncidentResponseContext _context;

        public IncidentEventRepository(IncidentResponseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IncidentEventModel>> GetAllAsync()
        {
            return await _context.IncidentEvents.ToListAsync();
        }

        public async Task<IncidentEventModel> GetByIdAsync(int incidentId, int eventId)
        {
            return await _context.IncidentEvents.FindAsync(incidentId, eventId);
        }

        public async Task AddAsync(IncidentEventModel incidentEventModel)
        {
            _context.IncidentEvents.Add(incidentEventModel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int incidentId, int eventId)
        {
            var incidentEventModel = await _context.IncidentEvents.FindAsync(incidentId, eventId);
            if (incidentEventModel != null)
            {
                _context.IncidentEvents.Remove(incidentEventModel);
                await _context.SaveChangesAsync();
            }
        }
    }
}



