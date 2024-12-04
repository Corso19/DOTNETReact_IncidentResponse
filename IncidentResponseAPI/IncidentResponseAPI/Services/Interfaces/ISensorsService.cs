using IncidentResponseAPI.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ISensorsService
{
    Task<IEnumerable<SensorsDto>> GetAllAsync();
    Task<SensorsDto> GetByIdAsync(int id);
    Task AddAsync(SensorsDto sensorsDto);
    Task UpdateAsync(int id, SensorsDto sensorsDto);
    Task DeleteAsync(int id);
    Task SetEnabledAsync(int id, bool isEnabled);
    
}