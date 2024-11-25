using IncidentResponseAPI.Dtos;

namespace IncidentResponseAPI.Services.Interfaces
{
    public interface IIncidentsService
    {
        Task<IEnumerable<IncidentsDto>> GetAllAsync();
        Task<IncidentsDto> GetByIdAsync(int id);
        Task AddAsync(IncidentsDto incidentsDto);
        Task UpdateAsync(int id, IncidentsDto incidentsDto);
        Task DeleteAsync(int id);
    }
}
