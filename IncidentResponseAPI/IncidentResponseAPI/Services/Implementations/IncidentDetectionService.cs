﻿using System.Security.Cryptography.Pkcs;
using IncidentResponseAPI.Constants;
using IncidentResponseAPI.Models;
using IncidentResponseAPI.Repositories.Interfaces;
using IncidentResponseAPI.Services.Interfaces;

namespace IncidentResponseAPI.Services.Implementations
{
    public class IncidentDetectionService : IIncidentDetectionService
    {
        private readonly IIncidentsRepository _incidentsRepository;
        private readonly IEventsRepository _eventsRepository;
        private readonly ILogger<IncidentDetectionService> _logger;

        public IncidentDetectionService(IIncidentsRepository incidentsRepository, IEventsRepository eventsRepository, ILogger<IncidentDetectionService> logger)
        {
            _incidentsRepository = incidentsRepository;
            _eventsRepository = eventsRepository;
            _logger = logger;
        }

        public async Task<bool> Detect(EventsModel @event, CancellationToken cancellationToken)
        {
            var incidentsCreated = false;

            try
            {
                if (HasSuspiciousAttachment(@event))
                {
                    await CreateIncident(@event, IncidentType.SuspiciousAttachment, cancellationToken);
                    incidentsCreated = true;
                }
                if (IsExternalSender(@event))
                {
                    await CreateIncident(@event, IncidentType.ExternalSender, cancellationToken);
                    incidentsCreated = true;
                }
                if (await IsRepeatedEventPatternAsync(@event, cancellationToken))
                {
                    await CreateIncident(@event, IncidentType.RepeatedEventPattern, cancellationToken);
                    incidentsCreated = true;
                }
                if (await HasUnusualEmailVolumeAsync(@event, cancellationToken))
                {
                    await CreateIncident(@event, IncidentType.UnusualEmailVolume, cancellationToken);
                    incidentsCreated = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during incident detection.");
            }

            return incidentsCreated;
        }

        private bool HasSuspiciousAttachment(EventsModel @event)
        {
            return @event.Attachments.Any(a =>
                a.Name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) ||
                a.Name.EndsWith(".bat", StringComparison.OrdinalIgnoreCase) ||
                a.Name.EndsWith(".ps1", StringComparison.OrdinalIgnoreCase) ||
                a.Name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase));
        }

        private bool IsExternalSender(EventsModel @event)
        {
            var trustedDomains = new List<string> { "5smw12.onmicrosoft.com" };
            var senderDomain = @event.Sender?.Split('@').Last();
            return senderDomain != null && !trustedDomains.Contains(senderDomain);
        }

        private async Task<bool> IsRepeatedEventPatternAsync(EventsModel @event, CancellationToken cancellationToken)
        {
            var hasSameSubject = await HasSameSubjectPatternAsync(@event, cancellationToken);
            var hasTimeBasedPattern = await HasTimeBasedPatternAsync(@event, cancellationToken);

            return hasSameSubject || hasTimeBasedPattern;
        }

        private async Task<bool> HasSameSubjectPatternAsync(EventsModel @event, CancellationToken cancellationToken)
        {
            try
            {
                var recentEvents = await _eventsRepository.GetEventsBySubjectAsync(@event.Subject, cancellationToken);
                return recentEvents.Count() >= 5;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in HasSameSubjectPatternAsync.");
                return false;
            }
        }

        private async Task<bool> HasTimeBasedPatternAsync(EventsModel @event, CancellationToken cancellationToken)
        {
            try
            {
                var matchingEvents = await _eventsRepository.GetEventsByTimestampAsync(@event.Timestamp, cancellationToken);
                return matchingEvents.Count() >= 3;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in HasTimeBasedPatternAsync.");
                return false;
            }
        }

        private async Task<bool> HasUnusualEmailVolumeAsync(EventsModel @event, CancellationToken cancellationToken)
        {
            try
            {
                var recentEvents = await _eventsRepository.GetEventsBySenderAsync(@event.Sender, cancellationToken);
                return recentEvents.Count() >= 10;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in HasUnusualEmailVolumeAsync.");
                return false;
            }
        }

        private async Task CreateIncident(EventsModel @event, IncidentType incidentType, CancellationToken cancellationToken)
        {
            var (severity, description) = IncidentTypeMetadata.GetMetadata(incidentType);

            var incident = new IncidentsModel
            {
                Title = incidentType.ToString(),
                Description = description,
                Severity = severity,
                Type = incidentType,
                DetectedAt = DateTime.Now,
                Status = "Open",
                EventId = @event.EventId,
                Event = @event // establish relationship with EventsModel
            };
            _logger.LogInformation("Creating incident for event with ID {EventId}", @event.EventId);

            try
            {
                await _incidentsRepository.AddAsync(incident, cancellationToken);
                _logger.LogInformation("Incident created for event with ID {EventId}", @event.EventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating incident.");
            }
        }
    }
}