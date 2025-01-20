using IncidentResponseAPI.Models;
using Microsoft.EntityFrameworkCore;
using IncidentResponseAPI.Repositories.Interfaces;

namespace IncidentResponseAPI.Repositories.Implementations
{
    public class IncidentsRepository : IIncidentsRepository
    {
        private readonly IncidentResponseContext _context;

        public IncidentsRepository(IncidentResponseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IncidentsModel>> GetAllAsync(bool includeEvent = false)
        {
            if (includeEvent)
            {
                return await _context.Incidents
                    .Include(i => i.Event)
                    .ToListAsync();
            }
            return await _context.Incidents.ToListAsync();
        }

        public async Task<IncidentsModel> GetByIdAsync(int id)
        {
            return await _context.Incidents.FindAsync(id);
        }

        public async Task AddAsync(IncidentsModel incidentsModel, CancellationToken cancellationToken)
        {
            _context.Incidents.Add(incidentsModel);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(IncidentsModel incidentsModel)
        {
            _context.Entry(incidentsModel).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var incidentsModel = await _context.Incidents.FindAsync(id);
            if (incidentsModel != null)
            {
                _context.Incidents.Remove(incidentsModel);
                await _context.SaveChangesAsync();
            }
        }
    }
}
