// using System.Collections.Concurrent;
// using IncidentResponseAPI.Models;
// using IncidentResponseAPI.Services.Interfaces;
// namespace IncidentResponseAPI.Orchestrators;

// public class SensorsOrchestrator : BackgroundService
// {
//     private readonly IServiceScopeFactory _scopeFactory;
//     private readonly ILogger<SensorsOrchestrator> _logger;
//     private readonly ConcurrentQueue<SensorsModel> _sensorsQueue;
//     private readonly SemaphoreSlim _semaphore;
//     private const int MaxConcurrentSensors = 5;
//     private const int DelayBetweenSensorRuns = 20;
//     private CancellationTokenSource _orchestratorCts;

//     public bool IsRunning { get; set; } 

//     public SensorsOrchestrator(IServiceScopeFactory scopeFactory, ILogger<SensorsOrchestrator> logger)
//     {
//         _scopeFactory = scopeFactory;
//         _logger = logger;
//         _sensorsQueue = new ConcurrentQueue<SensorsModel>();
//         _semaphore = new SemaphoreSlim(MaxConcurrentSensors);
//         _orchestratorCts = new CancellationTokenSource();
//         IsRunning = false;
//     }

//     public void EnqueueSensor(SensorsModel sensor)
//     {
//         _sensorsQueue.Enqueue(sensor);
//         ProcessQueue();
//     }

//     public void CancelAllSensors()
//     {
//         _orchestratorCts.Cancel();
//         //_orchestratorCts.Dispose();
//         _orchestratorCts = new CancellationTokenSource();
//         IsRunning = false;
//     }

//     private async void ProcessQueue()
//     {
//         IsRunning = true; // Update status
//         while (_sensorsQueue.Count > 0)
//         {
//             await _semaphore.WaitAsync();
//             if (_sensorsQueue.TryDequeue(out var sensor))
//             {
//                 _ = Task.Run(async () =>
//                 {
//                     var endTime = DateTime.UtcNow.AddMinutes(sensor.RetrievalInterval);
//                     try
//                     {
//                         using (var scope = _scopeFactory.CreateScope())
//                         {
//                             var sensorsService = scope.ServiceProvider.GetRequiredService<ISensorsService>();

//                             while (DateTime.UtcNow < endTime && !_orchestratorCts.Token.IsCancellationRequested)
//                             {
//                                 var freshSensorDto = await sensorsService.GetByIdAsync(sensor.SensorId);
//                                 if (freshSensorDto == null)
//                                 {
//                                     _logger.LogError("Sensor {SensorId} not found", sensor.SensorId);
//                                     break;
//                                 }

//                                 var freshSensor = new SensorsModel
//                                 {
//                                     SensorId = freshSensorDto.SensorId,
//                                     SensorName = freshSensorDto.SensorName,
//                                     Type = freshSensorDto.Type,
//                                     Configuration = freshSensorDto.Configuration,
//                                     isEnabled = freshSensorDto.isEnabled,
//                                     CreatedAt = freshSensorDto.CreatedAt,
//                                     LastRunAt = freshSensorDto.LastRunAt,
//                                     NextRunAfter = freshSensorDto.NextRunAfter,
//                                     LastError = freshSensorDto.LastError,
//                                     RetrievalInterval = freshSensorDto.RetrievalInterval,
//                                     LastEventMarker = freshSensorDto.LastEventMarker
//                                 };

//                                 await sensorsService.RunSensorAsync(freshSensor, _orchestratorCts.Token);
//                                 _logger.LogInformation("Sensor {SensorId} run completed at {Time}",
//                                     sensor.SensorId, DateTime.UtcNow);

//                                 await Task.Delay(TimeSpan.FromSeconds(DelayBetweenSensorRuns), _orchestratorCts.Token);
//                             }
//                         }
//                     }
//                     finally
//                     {
//                         _semaphore.Release();
//                         if (_sensorsQueue.IsEmpty)
//                         {
//                             IsRunning = false; // Update status
//                         }
//                     }
//                 });
//             }
//         }
//     }

//     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//     {
//         while (!stoppingToken.IsCancellationRequested)
//         {
//             await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
//         }
//     }
// }

using System.Collections.Concurrent;
using IncidentResponseAPI.Models;
using IncidentResponseAPI.Services.Interfaces;

namespace IncidentResponseAPI.Orchestrators;

public class SensorsOrchestrator : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SensorsOrchestrator> _logger;
    private readonly ConcurrentQueue<SensorsModel> _sensorsQueue;
    private readonly SemaphoreSlim _semaphore;
    private const int MaxConcurrentSensors = 5;
    private const int DelayBetweenSensorRuns = 20;
    private CancellationTokenSource _orchestratorCts;

    public bool IsRunning { get; set; }

    public SensorsOrchestrator(IServiceScopeFactory scopeFactory, ILogger<SensorsOrchestrator> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _sensorsQueue = new ConcurrentQueue<SensorsModel>();
        _semaphore = new SemaphoreSlim(MaxConcurrentSensors);
        _orchestratorCts = new CancellationTokenSource();
        IsRunning = false;
    }

    public void EnqueueSensor(SensorsModel sensor)
    {
        _sensorsQueue.Enqueue(sensor);
        ProcessQueue();
    }

    private async void ProcessQueue()
    {
        IsRunning = true;
        while (_sensorsQueue.Count > 0)
        {
            await _semaphore.WaitAsync();
            if (_sensorsQueue.TryDequeue(out var sensor))
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var sensorsService = scope.ServiceProvider.GetRequiredService<ISensorsService>();
                            while (!_orchestratorCts.Token.IsCancellationRequested)
                            {
                                var freshSensorDto = await sensorsService.GetByIdAsync(sensor.SensorId);
                                if (freshSensorDto == null)
                                {
                                    _logger.LogError("Sensor {SensorId} not found", sensor.SensorId);
                                    break;
                                }

                                if (!freshSensorDto.isEnabled)
                                {
                                    _logger.LogInformation("Sensor {SensorId} is disabled", sensor.SensorId);
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

                                await sensorsService.RunSensorAsync(freshSensor, _orchestratorCts.Token);
                                _logger.LogInformation("Sensor {SensorId} run completed", sensor.SensorId);

                                // Re-enqueue the sensor if still enabled
                                _sensorsQueue.Enqueue(sensor);

                                await Task.Delay(TimeSpan.FromSeconds(DelayBetweenSensorRuns), _orchestratorCts.Token);
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
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!IsRunning)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var sensorsService = scope.ServiceProvider.GetRequiredService<ISensorsService>();
                
                    var enabledSensors = await sensorsService.GetAllEnabledAsync();
                    foreach (var sensor in enabledSensors)
                    {
                        _sensorsQueue.Enqueue(sensor);
                    }
                
                    if (_sensorsQueue.Any())
                    {
                        IsRunning = true;
                        ProcessQueue();
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            // Log graceful shutdown
            _logger.LogInformation("Sensor orchestrator stopping");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in orchestrator background service");
        }
    }
}