﻿using IncidentResponseAPI.Repositories.Interfaces;
using IncidentResponseAPI.Services.Interfaces;

namespace IncidentResponseAPI.Services.Implementations;

public class EventsProcessingService : IEventsProcessingService
{
    private readonly IEventsRepository _eventsRepository;
    private readonly IIncidentDetectionService _incidentDetectionService;
    //TODO - Check if syncing events is needed or not

    public EventsProcessingService(IEventsRepository eventsRepository, IIncidentDetectionService incidentDetectionService)
    {
        _eventsRepository = eventsRepository;
        _incidentDetectionService = incidentDetectionService;
    }

    public async Task ProcessEventsAsync()
    {
        var unprocessedEvents = await _eventsRepository.GetUnprocessedEventsAsync();

        foreach (var @event in unprocessedEvents)
        {
            //Forward to detection service
            await _incidentDetectionService.Detect(@event);
            
            //Mark event as processed
            @event.isProcessed = true;
            await _eventsRepository.UpdateAsync(@event);
        }
    }
}