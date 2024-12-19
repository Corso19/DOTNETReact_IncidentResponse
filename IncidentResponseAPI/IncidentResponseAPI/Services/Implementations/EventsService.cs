using System.Net;
using IncidentResponseAPI.Dtos;
using IncidentResponseAPI.Models;
using IncidentResponseAPI.Repositories;
using IncidentResponseAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;

namespace IncidentResponseAPI.Services.Implementations
{
    public class EventsService : IEventsService
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly GraphAuthProvider _graphAuthProvider;
        private readonly ILogger<EventsService> _logger;

        public EventsService(
            IEventsRepository eventsRepository,
            IAttachmentRepository attachmentRepository,
            GraphAuthProvider graphAuthProvider,
            ILogger<EventsService> logger)
        {
            _eventsRepository = eventsRepository;
            _attachmentRepository = attachmentRepository;
            _graphAuthProvider = graphAuthProvider;
            _logger = logger;
        }

        public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
        {
            _logger.LogInformation("Fetching all events");
            try
            {
                var events = await _eventsRepository.GetAllAsync();

                var eventDtos = new List<EventDto>();
                foreach (var e in events)
                {
                    var attachments = await _attachmentRepository.GetAttachmentsByEventIdAsync(e.EventId);
                    eventDtos.Add(MapToDto(e, attachments));
                }

                _logger.LogInformation("Successfully fetched {Count} events", eventDtos.Count);
                return eventDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all events");
                throw;
            }
        }

        public async Task<EventDto> GetEventByIdAsync(int eventId)
        {
            _logger.LogInformation("Fetching event with ID {EventId}", eventId);
            try
            {
                var eventModel = await _eventsRepository.GetByIdAsync(eventId);
                if (eventModel == null)
                {
                    _logger.LogWarning("Event with ID {EventId} not found", eventId);
                    return null;
                }

                var attachments = await _attachmentRepository.GetAttachmentsByEventIdAsync(eventId);
                _logger.LogInformation("Successfully fetched event with ID {EventId}", eventId);
                return MapToDto(eventModel, attachments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching event with ID {EventId}", eventId);
                throw;
            }
        }

        public async Task AddEventAsync(EventDto eventDto)
        {
            _logger.LogInformation("Adding new event with Subject: {Subject}", eventDto.Subject);
            try
            {
                var eventModel = MapToModel(eventDto);
                await _eventsRepository.AddAsync(eventModel);

                if (eventDto.Attachments != null && eventDto.Attachments.Any())
                {
                    foreach (var attachment in eventDto.Attachments)
                    {
                        var attachmentModel = new AttachmentModel
                        {
                            EventId = eventModel.EventId,
                            Name = attachment.Name,
                            Size = attachment.Size,
                            Content = attachment.Content
                        };

                        await _attachmentRepository.AddAttachmentAsync(attachmentModel);
                    }
                }

                _logger.LogInformation("Successfully added event with Subject: {Subject}", eventDto.Subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding event with Subject: {Subject}", eventDto.Subject);
                throw;
            }
        }

        public async Task UpdateEventAsync(EventDto eventDto)
        {
            _logger.LogInformation("Updating event with ID {EventId}", eventDto.EventId);
            try
            {
                var eventModel = MapToModel(eventDto);
                await _eventsRepository.UpdateAsync(eventModel);

                _logger.LogInformation("Successfully updated event with ID {EventId}", eventDto.EventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating event with ID {EventId}", eventDto.EventId);
                throw;
            }
        }

        public async Task DeleteEventAsync(int eventId)
        {
            _logger.LogInformation("Deleting event with ID {EventId}", eventId);
            try
            {
                var attachments = await _attachmentRepository.GetAttachmentsByEventIdAsync(eventId);
                foreach (var attachment in attachments)
                {
                    await _attachmentRepository.DeleteAttachmentAsync(attachment.AttachmentId);
                }

                await _eventsRepository.DeleteAsync(eventId);
                _logger.LogInformation("Successfully deleted event with ID {EventId}", eventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting event with ID {EventId}", eventId);
                throw;
            }
        }

        public async Task SyncEventsAsync(string userId)
        {
            _logger.LogInformation("Starting sync for user: {UserId}", userId);
            try
            {
                var graphClient = await _graphAuthProvider.GetAuthenticatedGraphClient();
                var messages = await graphClient.Users[userId].MailFolders["Inbox"].Messages.GetAsync();

                foreach (var message in messages.Value)
                {
                    if (await _eventsRepository.GetByMessageIdAsync(message.Id) == null)
                    {
                        _logger.LogInformation("Adding new event for message ID: {MessageId}", message.Id);

                        var eventModel = new EventsModel
                        {
                            SensorId = 1,
                            TypeName = "Email",
                            Subject = message.Subject ?? "No subject",
                            Sender = message.Sender?.EmailAddress?.Address ?? "Unknown Sender",
                            Details = message.Body?.Content ?? "No Content",
                            Timestamp = message.ReceivedDateTime?.UtcDateTime ?? DateTime.Now,
                            isProcessed = false,
                            MessageId = message.Id
                        };

                        await _eventsRepository.AddAsync(eventModel);

                        var attachments = await graphClient.Users[userId].Messages[message.Id].Attachments.GetAsync();
                        foreach (var attachment in attachments.Value.OfType<FileAttachment>())
                        {
                            var newAttachment = new AttachmentModel
                            {
                                Name = attachment.Name ?? "Unnamed Attachment",
                                Size = attachment.Size ?? 0,
                                Content = attachment.ContentBytes,
                                EventId = eventModel.EventId
                            };

                            await _attachmentRepository.AddAttachmentAsync(newAttachment);
                        }
                    }
                }

                _logger.LogInformation("Sync completed for user: {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while syncing events for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<Message> FetchMessageContentAsync(string userId, string messageId)
        {
            _logger.LogInformation("Fetching message content for user: {UserId}, message ID: {MessageId}", userId, messageId);
            try
            {
                var graphClient = await _graphAuthProvider.GetAuthenticatedGraphClient();
                return await graphClient.Users[userId].Messages[messageId].GetAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching message content for user: {UserId}, message ID: {MessageId}", userId, messageId);
                throw;
            }
        }

        public async Task<IEnumerable<AttachmentDto>> GetAttachmentsByEventIdAsync(int eventId)
        {
            _logger.LogInformation("Fetching attachments for event ID: {EventId}", eventId);
            try
            {
                var attachments = await _attachmentRepository.GetAttachmentsByEventIdAsync(eventId);
                _logger.LogInformation("Successfully fetched {Count} attachments for event ID: {EventId}", attachments.Count(), eventId);
                return attachments.Select(a => new AttachmentDto
                {
                    AttachmentId = a.AttachmentId,
                    Name = a.Name,
                    Size = a.Size,
                    Content = a.Content,
                    EventId = a.EventId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching attachments for event ID: {EventId}", eventId);
                throw;
            }
        }

        private EventDto MapToDto(EventsModel eventModel, IEnumerable<AttachmentModel> attachments)
        {
            return new EventDto
            {
                EventId = eventModel.EventId,
                SensorId = eventModel.SensorId,
                TypeName = eventModel.TypeName,
                Subject = eventModel.Subject,
                Sender = eventModel.Sender,
                Details = eventModel.Details,
                Timestamp = eventModel.Timestamp,
                isProcessed = eventModel.isProcessed,
                MessageId = eventModel.MessageId,
                Attachments = attachments.Select(attachment => new AttachmentDto
                {
                    AttachmentId = attachment.AttachmentId,
                    EventId = attachment.EventId,
                    Name = attachment.Name,
                    Size = attachment.Size,
                    Content = attachment.Content
                }).ToList()
            };
        }

        private EventsModel MapToModel(EventDto eventDto)
        {
            return new EventsModel
            {
                EventId = eventDto.EventId,
                SensorId = eventDto.SensorId,
                TypeName = eventDto.TypeName,
                Subject = eventDto.Subject,
                Sender = eventDto.Sender,
                Details = eventDto.Details,
                Timestamp = eventDto.Timestamp,
                isProcessed = eventDto.isProcessed,
                MessageId = eventDto.MessageId,
                Attachments = eventDto.Attachments.Select(a => new AttachmentModel
                {
                    AttachmentId = a.AttachmentId,
                    EventId = a.EventId,
                    Name = a.Name,
                    Size = a.Size,
                    Content = a.Content
                }).ToList()
            };
        }
    }
}