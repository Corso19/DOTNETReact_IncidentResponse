using IncidentResponseAPI.Models;

namespace IncidentResponseAPI.Repositories.Interfaces
{
    public interface IIncidentsRepository
    {
        Task<IEnumerable<IncidentsModel>> GetAllAsync();
        Task<IncidentsModel> GetByIdAsync(int id);
        Task AddAsync(IncidentsModel incidentsModel);
        Task UpdateAsync(IncidentsModel incidentsModel);
        Task DeleteAsync(int id);
    }
}
