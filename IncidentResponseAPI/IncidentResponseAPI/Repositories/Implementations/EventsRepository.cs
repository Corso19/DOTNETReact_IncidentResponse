using IncidentResponseAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IncidentResponseAPI.Repositories.Implementations
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
            return await _context.Events
                .Include(e => e.Sensor)
                .Include(e =>e.Attachments)
                .ToListAsync();
        }

        public async Task<EventsModel> GetByIdAsync(int id)
        {
            return await _context.Events
                .Include(e => e.Sensor)
                .Include(e => e.Attachments)
                .FirstOrDefaultAsync(e => e.EventId == id);
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
        
        //Methods for attachments
        public async Task<IEnumerable<AttachmentModel>> GetAttachmentsByEventIdAsync(int eventId)
        {
            return await _context.Attachments
                .Where(a => a.EventId == eventId)
                .ToListAsync();
        }
        
        // public async Task AddAttachmentAsync(AttachmentModel attachmentModel)
        // {
        //     _context.Attachments.Add(attachmentModel);
        //     await _context.SaveChangesAsync();
        // }
    }
}



