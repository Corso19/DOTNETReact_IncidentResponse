using IncidentResponseAPI.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ISensorsService
{
    Task<IEnumerable<SensorDto>> GetAllAsync();
    Task<SensorDto> GetByIdAsync(int id);
    Task AddAsync(SensorDto sensorDto);
    Task UpdateAsync(int id, SensorDto sensorDto);
    Task DeleteAsync(int id);
    Task SetEnabledAsync(int id, bool isEnabled);
    
}