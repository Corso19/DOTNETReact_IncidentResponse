using IncidentResponseAPI.Dtos;


namespace IncidentResponseAPI.Services.Interfaces;
public interface ISensorsService
{
    Task<IEnumerable<SensorDto>> GetAllAsync();
    Task<SensorDto> GetByIdAsync(int id);
    Task<SensorDto> AddAsync(SensorDto sensorDto);
    Task UpdateAsync(int id, SensorDto sensorDto);
    Task DeleteAsync(int id);
    Task SetEnabledAsync(int id);
    
}