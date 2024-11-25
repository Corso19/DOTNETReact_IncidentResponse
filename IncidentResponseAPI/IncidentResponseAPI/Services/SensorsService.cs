using IncidentResponseAPI.Dtos;
using IncidentResponseAPI.Models;
using IncidentResponseAPI.Repositories;

namespace IncidentResponseAPI.Services
{
    public class SensorsService : ISensorsService
    {
        private readonly ISensorsRepository _sensorsRepository;

        public SensorsService(ISensorsRepository sensorsRepository)
        {
            _sensorsRepository = sensorsRepository;
        }

        public async Task<IEnumerable<SensorsDto>> GetAllAsync()
        {
            var sensors = await _sensorsRepository.GetAllAsync();
            return sensors.Select(s => new SensorsDto
            {
                SensorId = s.SensorId,
                SensorName = s.SensorName,
                Type = s.Type,
                ConfigurationJson = s.ConfigurationJson,
                isEnabled = s.isEnabled,
                CreatedAd = s.CreatedAd,
                LastRunAt = s.LastRunAt
            }).ToList();
        }

        public async Task<SensorsDto> GetByIdAsync(int id)
        {
            var s = await _sensorsRepository.GetByIdAsync(id);
            if (s == null) return null;

            return new SensorsDto
            {
                SensorId = s.SensorId,
                SensorName = s.SensorName,
                Type = s.Type,
                ConfigurationJson = s.ConfigurationJson,
                isEnabled = s.isEnabled,
                CreatedAd = s.CreatedAd,
                LastRunAt = s.LastRunAt
            };
        }

        public async Task AddAsync(SensorsDto sensorsDto)
        {
            var sensorsModel = new SensorsModel
            {
                SensorName = sensorsDto.SensorName,
                Type = sensorsDto.Type,
                ConfigurationJson = sensorsDto.ConfigurationJson,
                isEnabled = sensorsDto.isEnabled,
                CreatedAd = sensorsDto.CreatedAd,
                LastRunAt = sensorsDto.LastRunAt
            };

            await _sensorsRepository.AddAsync(sensorsModel);
        }

        public async Task UpdateAsync(int id, SensorsDto sensorsDto)
        {
            var sensorsModel = new SensorsModel
            {
                SensorId = id,
                SensorName = sensorsDto.SensorName,
                Type = sensorsDto.Type,
                ConfigurationJson = sensorsDto.ConfigurationJson,
                isEnabled = sensorsDto.isEnabled,
                CreatedAd = sensorsDto.CreatedAd,
                LastRunAt = sensorsDto.LastRunAt
            };

            await _sensorsRepository.UpdateAsync(sensorsModel);
        }

        public async Task DeleteAsync(int id)
        {
            await _sensorsRepository.DeleteAsync(id);
        }
    }
}

