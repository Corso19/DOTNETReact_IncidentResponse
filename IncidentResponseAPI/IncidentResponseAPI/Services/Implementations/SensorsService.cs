using IncidentResponseAPI.Dtos;
using IncidentResponseAPI.Models;
using IncidentResponseAPI.Repositories;

namespace IncidentResponseAPI.Services.Implementations
{
    public class SensorsService : ISensorsService
    {
        private readonly ISensorsRepository _sensorsRepository;

        public SensorsService(ISensorsRepository sensorsRepository)
        {
            _sensorsRepository = sensorsRepository;
        }

        public async Task<IEnumerable<SensorDto>> GetAllAsync()
        {
            var sensors = await _sensorsRepository.GetAllAsync();
            return sensors.Select(s => new SensorDto
            {
                SensorId = s.SensorId,
                SensorName = s.SensorName,
                Type = s.Type,
                TenantId = s.TenantId,
                ApplicationId = s.ApplicationId,
                ClientSecret = s.ClientSecret,
                isEnabled = s.isEnabled,
                CreatedAd = s.CreatedAd,
                LastRunAt = s.LastRunAt
            }).ToList();
        }

        public async Task<SensorDto> GetByIdAsync(int id)
        {
            var s = await _sensorsRepository.GetByIdAsync(id);
            if (s == null) return null;

            return new SensorDto
            {
                SensorId = s.SensorId,
                SensorName = s.SensorName,
                Type = s.Type,
                TenantId = s.TenantId,
                ApplicationId = s.ApplicationId,
                ClientSecret = s.ClientSecret,
                isEnabled = s.isEnabled,
                CreatedAd = s.CreatedAd,
                LastRunAt = s.LastRunAt
            };
        }

        public async Task AddAsync(SensorDto sensorDto)
        {
            var sensorsModel = new SensorsModel
            {
                SensorName = sensorDto.SensorName,
                Type = sensorDto.Type,
                TenantId = sensorDto.TenantId,
                ApplicationId = sensorDto.ApplicationId,
                ClientSecret = sensorDto.ClientSecret,
                isEnabled = sensorDto.isEnabled,
                CreatedAd = sensorDto.CreatedAd,
                LastRunAt = sensorDto.LastRunAt
            };

            await _sensorsRepository.AddAsync(sensorsModel);
        }

        public async Task UpdateAsync(int id, SensorDto sensorDto)
        {
            var sensorsModel = new SensorsModel
            {
                SensorId = id,
                SensorName = sensorDto.SensorName,
                Type = sensorDto.Type,
                TenantId = sensorDto.TenantId,
                ApplicationId = sensorDto.ApplicationId,
                ClientSecret = sensorDto.ClientSecret,
                isEnabled = sensorDto.isEnabled,
                CreatedAd = sensorDto.CreatedAd,
                LastRunAt = sensorDto.LastRunAt
            };

            await _sensorsRepository.UpdateAsync(sensorsModel);
        }

        public async Task DeleteAsync(int id)
        {
            await _sensorsRepository.DeleteAsync(id);
        }

        public async Task SetEnabledAsync(int id, bool isEnabled)
        {
            var sensor = await _sensorsRepository.GetByIdAsync(id);
            if (sensor != null)
            {
                sensor.isEnabled = isEnabled;
                await _sensorsRepository.UpdateAsync(sensor);
            }
        }
    }
}