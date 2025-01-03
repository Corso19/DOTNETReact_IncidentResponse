using IncidentResponseAPI.Repositories.Interfaces;
using IncidentResponseAPI.Services.Interfaces;

namespace IncidentResponseAPI.Services.Implementations;

public class EventsProcessingService(IEventsRepository eventsRepository) : IEventsProcessingService
{
    private readonly IEventsRepository _eventsRepository = eventsRepository;
    // private readonly IDetectionService _detectionService;

    //TODO - Add IDetectionService to constructor 

    public async Task ProcessEventsAsync()
    {
        var unprocessedEvents = await _eventsRepository.GetUnprocessedEventsAsync();

        foreach (var @event in unprocessedEvents)
        {
            //Forward to detection service, for now incomplete
            // await _detectionService.Detect(@event);
            
            //Mark event as processed
            @event.isProcessed = true;
            await _eventsRepository.UpdateAsync(@event);
        }
    }
}