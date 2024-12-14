using IncidentResponseAPI.Dtos;
using Microsoft.Graph.Models;

namespace IncidentResponseAPI.Services
{
    public interface IEventsService
    {
        //CRUD operations
        Task<IEnumerable<EventsDto>> GetAllAsync();
        Task<EventsDto> GetByIdAsync(int id);
        Task AddAsync(EventsDto eventDto);
        Task UpdateAsync(EventsDto eventDto);
        Task DeleteAsync(int id);
        
        //Email/Event-related operations
        Task SyncEventsAsync(string userId);
        Task <Message> FetchMessageContentAsync(string userid, string messageId);
        
        //Attachment-related operations
        Task<IEnumerable<AttachmentDto>> GetAttachmentsByEventIdAsync(int eventId);
        // Task AddAttachmentAsync(AttachmentDto attachmentDto);
        
    }
}




