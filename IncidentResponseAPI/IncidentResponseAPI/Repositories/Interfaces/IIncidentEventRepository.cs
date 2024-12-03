using IncidentResponseAPI.Models;

namespace IncidentResponseAPI.Repositories
{
    public interface IIncidentEventRepository
    {
        Task<IEnumerable<IncidentEventModel>> GetAllAsync();
        Task<IncidentEventModel> GetByIdAsync(int incidentId, int eventId);
        Task AddAsync(IncidentEventModel incidentEventModel);
        Task DeleteAsync(int incidentId, int eventId);
    }
}



