using IncidentResponseAPI.Dtos;
using Microsoft.Graph.Models;

namespace IncidentResponseAPI.Services.Interfaces
{
    public interface IEventsService
    {
        //CRUD operations
        Task<IEnumerable<EventsDto>> GetAllEventsAsync();
        Task<EventsDto> GetEventByIdAsync(int id);
        Task AddEventAsync(EventsDto eventDto);
        Task UpdateEventAsync(EventsDto eventDto);
        Task DeleteEventAsync(int id);
        
        //Email/Event-related operations
        Task SyncEventsAsync(string userId);
        Task <Message> FetchMessageContentAsync(string userid, string messageId);
        
        //Attachment-related operations
        Task<IEnumerable<AttachmentDto>> GetAttachmentsByEventIdAsync(int eventId);
        // Task AddAttachmentAsync(AttachmentDto attachmentDto);
        
    }
}




