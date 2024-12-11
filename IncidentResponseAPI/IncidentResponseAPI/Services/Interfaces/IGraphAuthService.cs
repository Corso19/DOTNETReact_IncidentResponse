using Microsoft.Graph.Models;

namespace IncidentResponseAPI.Services.Interfaces;

public interface IGraphAuthService
{
    Task<IEnumerable<User>> FetchUsersAsync();
    Task<IEnumerable<Message>> FetchEmailsAsync(string userId);
    Task<Message> FetchMessageContentAsync(string userId, string messageId);
    Task<IEnumerable<Attachment>> FetchAttachmentsAsync(string userId, string messageId);
}