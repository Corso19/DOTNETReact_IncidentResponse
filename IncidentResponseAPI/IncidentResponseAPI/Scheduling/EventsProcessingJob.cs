using IncidentResponseAPI.Services.Implementations;
using IncidentResponseAPI.Services.Interfaces;
using Quartz;

namespace IncidentResponseAPI.Scheduling;

public class EventsProcessingJob : IJob
{
    private readonly IEventsProcessingService _eventsProcessingService;
    private readonly ILogger<EventsProcessingJob> _logger;
    
    public EventsProcessingJob(IEventsProcessingService eventsProcessingService, ILogger<EventsProcessingJob> logger)
    {
        _eventsProcessingService = eventsProcessingService;
        _logger = logger;
        _logger.LogInformation("EventsProcessingJob created at {Time}", DateTime.Now);
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("EventsProcessingJob started at {Time}", DateTime.Now);
        try
        {
            await _eventsProcessingService.ProcessEventsAsync();
            _logger.LogInformation("EventsProcessingJob completed at {Time}", DateTime.Now);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "EventsProcessingJob failed at {Time}", DateTime.Now);
        }
    }
}