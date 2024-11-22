using IncidentResponseAPI.Models;

namespace IncidentResponseAPI.Repositories
{
    public interface IEventsRepository
    {
        Task<IEnumerable<EventsModel>> GetAllAsync();
        Task<EventsModel> GetByIdAsync(int id);
        Task AddAsync(EventsModel eventsModel);
        Task UpdateAsync(EventsModel eventsModel);
        Task DeleteAsync(int id);
    }
}




