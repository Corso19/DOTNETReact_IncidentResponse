using System.Collections.Concurrent;
using System.Threading.Tasks;
using IncidentResponseAPI.Models;
using IncidentResponseAPI.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IncidentResponseAPI.Orchestrators;

public class SensorsOrchestrator : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SensorsOrchestrator> _logger;
    private readonly ConcurrentQueue<SensorsModel> _sensorsQueue;
    private readonly SemaphoreSlim _semaphore;
    private const int MaxConcurrentSensors = 5;

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
                    try
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var sensorsService = scope.ServiceProvider.GetRequiredService<ISensorsService>();
                            await sensorsService.RunSensorAsync(sensor);
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