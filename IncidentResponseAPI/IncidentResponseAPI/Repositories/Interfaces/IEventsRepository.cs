using IncidentResponseAPI.Models;

namespace IncidentResponseAPI.Repositories.Interfaces
{
    public interface IEventsRepository
    {
        Task<IEnumerable<EventsModel>> GetAllAsync();
        Task<EventsModel> GetByIdAsync(int id);
        Task AddAsync(EventsModel eventsModel);
        Task UpdateAsync(EventsModel eventsModel);
        Task DeleteAsync(int id);
        Task<IEnumerable<EventsModel>> GetUnprocessedEventsAsync();
        
        Task<IEnumerable<AttachmentModel>> GetAttachmentsByEventIdAsync(int eventId);
        Task<EventsModel> GetByMessageIdAsync(string messageId);
        
    }
}




