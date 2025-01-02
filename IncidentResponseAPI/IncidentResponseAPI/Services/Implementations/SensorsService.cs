using IncidentResponseAPI.Dtos;
using IncidentResponseAPI.Models;
using IncidentResponseAPI.Repositories.Interfaces;
using IncidentResponseAPI.Services.Interfaces;
using Quartz.Xml;

namespace IncidentResponseAPI.Services.Implementations
{
    public class SensorsService : ISensorsService
    {
        private readonly ISensorsRepository _sensorsRepository;
        private readonly ILogger<SensorsService> _logger;
        private readonly IConfigurationValidator _configurationValidator;

        public SensorsService(ISensorsRepository sensorsRepository, ILogger<SensorsService> logger, IConfigurationValidator configurationValidator)
        {
            _sensorsRepository = sensorsRepository;
            _logger = logger;
            _configurationValidator = configurationValidator;
        }

        public async Task<IEnumerable<SensorDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all sensors.");
            try
            {
                var sensors = await _sensorsRepository.GetAllAsync();
                _logger.LogInformation("Successfully fetched all sensors.");
                return sensors.Select(s => new SensorDto
                {
                    SensorId = s.SensorId,
                    SensorName = s.SensorName,
                    Type = s.Type,
                    Configuration = s.Configuration,
                    isEnabled = s.isEnabled,
                    CreatedAt = s.CreatedAt,
                    LastRunAt = s.LastRunAt,
                    NextRunAfter = s.NextRunAfter,
                    LastError = s.LastError,
                    RetrievalInterval = s.RetrievalInterval,
                    LastEventMarker = s.LastEventMarker
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all sensors.");
                throw;
            }
        }

        public async Task<SensorDto> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching sensor with ID {SensorId}.", id);
            try
            {
                var s = await _sensorsRepository.GetByIdAsync(id);
                if (s == null)
                {
                    _logger.LogWarning("Sensor with ID {SensorId} not found.", id);
                    return null;
                }

                _logger.LogInformation("Successfully fetched sensor with ID {SensorId}.", id);
                return new SensorDto
                {
                    SensorId = s.SensorId,
                    SensorName = s.SensorName,
                    Type = s.Type,
                    Configuration = s.Configuration,
                    isEnabled = s.isEnabled,
                    CreatedAt = s.CreatedAt,
                    LastRunAt = s.LastRunAt,
                    NextRunAfter = s.NextRunAfter,
                    LastError = s.LastError,
                    RetrievalInterval = s.RetrievalInterval,
                    LastEventMarker = s.LastEventMarker
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching sensor with ID {SensorId}.", id);
                throw;
            }
        }

        public async Task AddAsync(SensorDto sensorDto)
        {
            _logger.LogInformation("Adding a new sensor.");
            try
            {
                _configurationValidator.Validate(sensorDto.Configuration);

                var sensorsModel = new SensorsModel
                {
                    SensorName = sensorDto.SensorName,
                    Type = sensorDto.Type,
                    Configuration = sensorDto.Configuration,
                    isEnabled = sensorDto.isEnabled,
                    CreatedAt = sensorDto.CreatedAt,
                    LastRunAt = sensorDto.LastRunAt,
                    NextRunAfter = sensorDto.NextRunAfter,
                    LastError = sensorDto.LastError,
                    RetrievalInterval = sensorDto.RetrievalInterval,
                    LastEventMarker = sensorDto.LastEventMarker
                };

                await _sensorsRepository.AddAsync(sensorsModel);
                _logger.LogInformation("Successfully added a new sensor.");
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation failed for new sensor");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new sensor.");
                throw;
            }
        }

        public async Task UpdateAsync(int id, SensorDto sensorDto)
        {
            _logger.LogInformation("Updating sensor with ID {SensorId}.", id);
            try
            {
                _configurationValidator.Validate(sensorDto.Configuration);

                var sensorsModel = new SensorsModel
                {
                    SensorId = id,
                    SensorName = sensorDto.SensorName,
                    Type = sensorDto.Type,
                    Configuration = sensorDto.Configuration,
                    isEnabled = sensorDto.isEnabled,
                    CreatedAt = sensorDto.CreatedAt,
                    LastRunAt = sensorDto.LastRunAt,
                    NextRunAfter = sensorDto.NextRunAfter,
                    LastError = sensorDto.LastError,
                    RetrievalInterval = sensorDto.RetrievalInterval,
                    LastEventMarker = sensorDto.LastEventMarker
                };

                await _sensorsRepository.UpdateAsync(sensorsModel);
                _logger.LogInformation("Successfully updated sensor with ID {SensorId}.", id);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation failed for sensor with ID {SensorId}.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating sensor with ID {SensorId}.", id);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting sensor with ID {SensorId}.", id);
            try
            {
                await _sensorsRepository.DeleteAsync(id);
                _logger.LogInformation("Successfully deleted sensor with ID {SensorId}.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting sensor with ID {SensorId}.", id);
                throw;
            }
        }

        public async Task SetEnabledAsync(int id, bool isEnabled)
        {
            _logger.LogInformation("Setting enabled status for sensor with ID {SensorId} to {IsEnabled}.", id, isEnabled);
            try
            {
                var sensor = await _sensorsRepository.GetByIdAsync(id);
                if (sensor != null)
                {
                    sensor.isEnabled = isEnabled;
                    await _sensorsRepository.UpdateAsync(sensor);
                    _logger.LogInformation("Successfully updated enabled status for sensor with ID {SensorId} to {IsEnabled}.", id, isEnabled);
                }
                else
                {
                    _logger.LogWarning("Sensor with ID {SensorId} not found. Cannot set enabled status.", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while setting enabled status for sensor with ID {SensorId}.", id);
                throw;
            }
        }
    }
}