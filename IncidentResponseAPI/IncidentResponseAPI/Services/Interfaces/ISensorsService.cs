using IncidentResponseAPI.Dtos;

namespace IncidentResponseAPI.Services
{
    public interface ISensorsService
    {
        Task<IEnumerable<SensorsDto>> GetAllAsync();
        Task<SensorsDto> GetByIdAsync(int id);
        Task AddAsync(SensorsDto sensorsDto);
        Task UpdateAsync(int id, SensorsDto sensorsDto);
        Task DeleteAsync(int id);
    }
}

