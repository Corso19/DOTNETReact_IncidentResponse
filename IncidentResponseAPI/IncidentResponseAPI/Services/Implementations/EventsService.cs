﻿using System.Net;
using IncidentResponseAPI.Dtos;
using IncidentResponseAPI.Models;
using IncidentResponseAPI.Repositories;
using IncidentResponseAPI.Services.Interfaces;
using Microsoft.Graph.Models;

namespace IncidentResponseAPI.Services.Implementations
{
    public class EventsService : IEventsService
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly GraphAuthProvider _graphAuthProvider;
        
        public EventsService(
            IEventsRepository eventsRepository,
            IAttachmentRepository attachmentRepository,
            GraphAuthProvider graphAuthProvider)
        {
            _eventsRepository = eventsRepository;
            _attachmentRepository = attachmentRepository;
            _graphAuthProvider = graphAuthProvider;
        }

        //CRUD operations for Events
        // public async Task<IEnumerable<EventsDto>> GetAllEventsAsync()
        // {
        //     var events = await _eventsRepository.GetAllAsync();
        //     return await Task.WhenAll(events.Select(async e =>
        //     {
        //         var attachments = await _attachmentRepository.GetAttachmentsByEventIdAsync(e.EventId);
        //         return MapToDto(e, attachments);
        //     }));
        // }
        
        public async Task<IEnumerable<EventsDto>> GetAllEventsAsync()
        {
            var events = await _eventsRepository.GetAllAsync();

            // Fetch attachments separately to avoid concurrency issues
            var eventDtos = new List<EventsDto>();
            foreach (var e in events)
            {
                var attachments = await _attachmentRepository.GetAttachmentsByEventIdAsync(e.EventId);
                eventDtos.Add(MapToDto(e, attachments));
            }

            return eventDtos;
        }


        public async Task<EventsDto> GetEventByIdAsync(int eventId)
        {
            var eventModel = await _eventsRepository.GetByIdAsync(eventId);
            if (eventModel == null) return null;
            
            var attachments = await _attachmentRepository.GetAttachmentsByEventIdAsync(eventId);
            return MapToDto(eventModel, attachments);
        }
        
        public async Task AddEventAsync(EventsDto eventDto)
        {
            var eventModel = MapToModel(eventDto);
            await _eventsRepository.AddAsync(eventModel);
            
            //Add attachment if present in the DTO
            if(eventDto.Attachments != null && eventDto.Attachments.Any())
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
        }
        
        public async Task UpdateEventAsync(EventsDto eventDto)
        {
            var eventModel = MapToModel(eventDto);
            await _eventsRepository.UpdateAsync(eventModel);
            
            // Attachments remain immutable, so no updates are performed on attachments - this can be subject to change
        }
        
        public async Task DeleteEventAsync(int eventId)
        {
            var attachments = await _attachmentRepository.GetAttachmentsByEventIdAsync(eventId);
            
            foreach (var attachment in attachments)
            {
                await _attachmentRepository.DeleteAttachmentAsync(attachment.AttachmentId);
            }
            
            await _eventsRepository.DeleteAsync(eventId);
        }
        
        //Email-related operations
        public async Task SyncEventsAsync(string userId)
        {
            var graphClient = await _graphAuthProvider.GetAuthenticatedGraphClient();
            var messages = await graphClient.Users[userId].MailFolders["Inbox"].Messages.GetAsync();

            foreach(var message in messages.Value)
            {
                if(await _eventsRepository.GetByMessageIdAsync(message.Id) == null)
                {
                    var eventModel = new EventsModel
                    {
                        SensorId = 1, //default sensor ID
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
                            Name = attachment.Name ?? "Unname Attachment",
                            Size = attachment.Size ?? 0,
                            Content = attachment.ContentBytes,
                            EventId = eventModel.EventId
                        };
                        
                        await _attachmentRepository.AddAttachmentAsync(newAttachment);
                    }
                }
            }
        }
        
        public async Task<Message> FetchMessageContentAsync(string userId, string messageId)
        {
            var graphClient = await _graphAuthProvider.GetAuthenticatedGraphClient();
            return await graphClient.Users[userId].Messages[messageId].GetAsync();
        }
        
        //Attachment-related operations
        
        public async Task<IEnumerable<AttachmentDto>> GetAttachmentsByEventIdAsync(int eventId)
        {
            var attachments = await _attachmentRepository.GetAttachmentsByEventIdAsync(eventId);
            return attachments.Select(a => new AttachmentDto
            {
                AttachmentId = a.AttachmentId,
                Name = a.Name,
                Size = a.Size,
                Content = a.Content, //include the content for analysis
                EventId = a.EventId
            });
        }
        
        //Helper methods

        private EventsDto MapToDto(EventsModel eventModel, IEnumerable<AttachmentModel> attachments)
        {
            return new EventsDto
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
                Attachments = eventModel.Attachments.Select(attachment => new AttachmentDto
                {
                    AttachmentId = attachment.AttachmentId,
                    EventId = attachment.EventId,
                    Name = attachment.Name,
                    Size = attachment.Size,
                    Content = attachment.Content
                }).ToList()
            };
        }

        private EventsModel MapToModel(EventsDto eventDto)
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



