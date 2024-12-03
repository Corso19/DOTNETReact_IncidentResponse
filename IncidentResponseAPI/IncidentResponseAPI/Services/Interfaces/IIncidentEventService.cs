using IncidentResponseAPI.Dtos;

namespace IncidentResponseAPI.Services
{
    public interface IIncidentEventService
    {
        Task<IEnumerable<IncidentEventDto>> GetAllAsync();
        Task<IncidentEventDto> GetByIdAsync(int incidentId, int eventId);
        Task AddAsync(IncidentEventDto incidentEventDto);
        Task DeleteAsync(int incidentId, int eventId);
    }
}



