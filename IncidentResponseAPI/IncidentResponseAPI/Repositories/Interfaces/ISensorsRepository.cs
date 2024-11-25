using IncidentResponseAPI.Models;

namespace IncidentResponseAPI.Repositories
{
    public interface ISensorsRepository
    {
        Task<IEnumerable<SensorsModel>> GetAllAsync();
        Task<SensorsModel> GetByIdAsync(int id);
        Task AddAsync(SensorsModel sensorsModel);
        Task UpdateAsync(SensorsModel sensorsModel);
        Task DeleteAsync(int id);
    }
}

