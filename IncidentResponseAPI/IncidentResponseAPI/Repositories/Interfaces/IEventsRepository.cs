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
        Task<IEnumerable<EventsModel>> GetEventsBySubjectAsync(string subject);
        Task<IEnumerable<EventsModel>> GetEventsByTimestampAsync(DateTime timestamp);
        Task<IEnumerable<EventsModel>> GetEventsBySenderAsync(string sender);
        
        Task<IEnumerable<AttachmentModel>> GetAttachmentsByEventIdAsync(int eventId);
        Task<EventsModel> GetByMessageIdAsync(string messageId);
        
    }
}




