using IncidentResponseAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IncidentResponseAPI.Repositories
{
    public class EventsRepository : IEventsRepository
    {
        private readonly IncidentResponseContext _context;

        public EventsRepository(IncidentResponseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventsModel>> GetAllAsync()
        {
            return await _context.Events.Include(e => e.Sensor).ToListAsync();
        }

        public async Task<EventsModel> GetByIdAsync(int id)
        {
            return await _context.Events.Include(e => e.Sensor).FirstOrDefaultAsync(e => e.EventId == id);
        }

        public async Task AddAsync(EventsModel eventsModel)
        {
            _context.Events.Add(eventsModel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EventsModel eventsModel)
        {
            _context.Entry(eventsModel).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var eventsModel = await _context.Events.FindAsync(id);
            if (eventsModel != null)
            {
                _context.Events.Remove(eventsModel);
                await _context.SaveChangesAsync();
            }
        }
    }
}

