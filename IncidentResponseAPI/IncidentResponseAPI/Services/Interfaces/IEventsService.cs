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
        Task UpdateAsync(int id, EventsDto eventDto);
        Task DeleteAsync(int id);
        
        //Email/Event-related operations
        Task SyncEventsAsync();
        Task <Message> FetchMessageContentAsync(string userid, string messageId);
        
        //Attachment-related operations
        
    }
}




