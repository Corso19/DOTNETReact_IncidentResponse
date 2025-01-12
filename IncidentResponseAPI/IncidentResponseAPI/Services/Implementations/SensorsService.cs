using IncidentResponseAPI.Dtos;
using IncidentResponseAPI.Models;
using IncidentResponseAPI.Repositories.Interfaces;
using IncidentResponseAPI.Services.Interfaces;
using Microsoft.Kiota.Abstractions;
using Quartz.Xml;

namespace IncidentResponseAPI.Services.Implementations
{
    public class SensorsService : ISensorsService
    {
        private readonly ISensorsRepository _sensorsRepository;
        private readonly ILogger<SensorsService> _logger;
        private readonly IConfigurationValidator _configurationValidator;
        private readonly IEventsService _eventsService;
        private readonly IEventsProcessingService _eventsProcessingService;

        public SensorsService(ISensorsRepository sensorsRepository, ILogger<SensorsService> logger, IConfigurationValidator configurationValidator, IEventsService eventsService, IEventsProcessingService eventsProcessingService)
        {
            _sensorsRepository = sensorsRepository;
            _logger = logger;
            _configurationValidator = configurationValidator;
            _eventsService = eventsService;
            _eventsProcessingService = eventsProcessingService;
        }
        

        public async Task RunSensorAsync(SensorsModel sensor)
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(sensor.RetrievalInterval)))
            {
                try
                {
                    //Sync new events
                    await _eventsService.SyncEventsAsync(sensor.SensorId, cts.Token);
                    //Process events to detect incidents
                    await _eventsProcessingService.ProcessEventsAsync(cts.Token);
                    
                    //Update LastRunAt and persist to the database
                    sensor.LastRunAt = DateTime.UtcNow;
                    await _sensorsRepository.UpdateAsync(sensor);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogWarning("Sensor with ID {SensorId} was canceled after {RetrievalInterval} minutes.", sensor.SensorId, sensor.RetrievalInterval);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while running sensor with ID {SensorId}.", sensor.SensorId);
                }
            }
        }

        public async Task<IEnumerable<SensorsModel>> GetAllEnabledAsync()
        {
            _logger.LogInformation("Fetching all enabled sensors.");
            try
            {
                var sensors = await _sensorsRepository.GetAllEnabledAsync();
                _logger.LogInformation("Successfully fetched all enabled sensors.");
                return sensors;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all enabled sensors.");
                throw;
            }
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

        public async Task<SensorDto> AddAsync(SensorDto sensorDto)
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

                sensorDto.SensorId = sensorsModel.SensorId;
                return sensorDto;
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation failed for new sensor");
                throw;
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

        public async Task SetEnabledAsync(int id)
        {
            _logger.LogInformation("Setting enabled status for sensor with ID {SensorId}.", id);
            try
            {
                var sensor = await _sensorsRepository.GetByIdAsync(id);
                if (sensor != null)
                {
                    sensor.isEnabled = !sensor.isEnabled;
                    await _sensorsRepository.UpdateAsync(sensor);
                    _logger.LogInformation("Successfully toggled enabled status for sensor with ID {SensorId} to {IsEnabled}.", id, sensor.isEnabled);
                }
                else
                {
                    _logger.LogWarning("Sensor with ID {SensorId} not found. Cannot set enabled status.", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while toggling enabled status for sensor with ID {SensorId}.", id);
                throw;
            }
        }
    }
}