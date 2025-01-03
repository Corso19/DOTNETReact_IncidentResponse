using IncidentResponseAPI.Repositories.Interfaces;

namespace IncidentResponseAPI.Services.Implementations;

public class EventsProcessingService
{
    private readonly IEventsRepository _eventsRepository;
    // private readonly IDetectionService _detectionService;
    
    public EventsProcessingService(IEventsRepository eventsRepository)
    {
        _eventsRepository = eventsRepository;
        //TODO - Add IDetectionService to constructor 
    }
    
    public async Task ProcessEvents()
    {
        var unprocessedEvents = await _eventsRepository.GetUnprocessedEventsAsync();

        foreach (var @event in unprocessedEvents)
        {
            //Forward to detection service, for now incomplete'
            // await _detectionService.Detect(@event);
            
            //Mark event as processed
            @event.isProcessed = true;
            await _eventsRepository.UpdateAsync(@event);
        }
    }
}