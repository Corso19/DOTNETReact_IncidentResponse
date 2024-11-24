using IncidentResponseAPI.Dtos;

namespace IncidentResponseAPI.Services
{
    public interface IEventsService
    {
        Task<IEnumerable<EventsDto>> GetAllAsync();
        Task<EventsDto> GetByIdAsync(int id);
        Task AddAsync(EventsDto eventDto);
        Task UpdateAsync(int id, EventsDto eventDto);
        Task DeleteAsync(int id);
    }
}




