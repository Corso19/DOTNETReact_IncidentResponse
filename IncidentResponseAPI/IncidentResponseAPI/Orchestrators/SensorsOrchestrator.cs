using System.Collections.Concurrent;
using System.Threading.Tasks;
using IncidentResponseAPI.Models;
using IncidentResponseAPI.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;

namespace IncidentResponseAPI.Orchestrators;

public class SensorsOrchestrator : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SensorsOrchestrator> _logger;
    private readonly ConcurrentQueue<SensorsModel> _sensorsQueue;
    private readonly SemaphoreSlim _semaphore;
    private const int MaxConcurrentSensors = 5;
    private const int DelayBetweenSensorRuns = 20;

    public SensorsOrchestrator(IServiceScopeFactory scopeFactory, ILogger<SensorsOrchestrator> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _sensorsQueue = new ConcurrentQueue<SensorsModel>();
        _semaphore = new SemaphoreSlim(MaxConcurrentSensors);
    }

    public void EnqueueSensor(SensorsModel sensor)
    {
        _sensorsQueue.Enqueue(sensor);
        ProcessQueue();
    }

    private async void ProcessQueue()
    {
        while (_sensorsQueue.Count > 0)
        {
            await _semaphore.WaitAsync();
            if (_sensorsQueue.TryDequeue(out var sensor))
            {
                _ = Task.Run(async () =>
                {
                    var endTime = DateTime.UtcNow.AddMinutes(sensor.RetrievalInterval);
                    try
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var sensorsService = scope.ServiceProvider.GetRequiredService<ISensorsService>();

                            while (DateTime.UtcNow < endTime)
                            {
                                // Get fresh sensor instance before each run
                                var freshSensorDto = await sensorsService.GetByIdAsync(sensor.SensorId);
                                if (freshSensorDto == null)
                                {
                                    _logger.LogError("Sensor {SensorId} not found", sensor.SensorId);
                                    break;
                                }

                                var freshSensor = new SensorsModel
                                {
                                    SensorId = freshSensorDto.SensorId,
                                    SensorName = freshSensorDto.SensorName,
                                    Type = freshSensorDto.Type,
                                    Configuration = freshSensorDto.Configuration,
                                    isEnabled = freshSensorDto.isEnabled,
                                    CreatedAt = freshSensorDto.CreatedAt,
                                    LastRunAt = freshSensorDto.LastRunAt,
                                    NextRunAfter = freshSensorDto.NextRunAfter,
                                    LastError = freshSensorDto.LastError,
                                    RetrievalInterval = freshSensorDto.RetrievalInterval,
                                    LastEventMarker = freshSensorDto.LastEventMarker
                                };

                                await sensorsService.RunSensorAsync(freshSensor);
                                _logger.LogInformation("Sensor {SensorId} run completed at {Time}",
                                    sensor.SensorId, DateTime.UtcNow);

                                await Task.Delay(TimeSpan.FromSeconds(DelayBetweenSensorRuns));
                            }
                        }
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                });
            }
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}