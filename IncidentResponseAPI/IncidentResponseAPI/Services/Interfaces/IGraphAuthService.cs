using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace IncidentResponseAPI.Services.Interfaces
{
    public interface IGraphAuthService
    {
        Task<Dictionary<string, List<Message>>> FetchEmailsForAllUsersAsync(
            string clientSecret, 
            string applicationId, 
            string tenantId, 
            DateTime? lastProcessedTime, 
            CancellationToken cancellationToken);
        Task<Message> FetchMessageContentAsync(string clientSecret, 
            string applicationId, 
            string tenantId, 
            string messageId);
        Task<IEnumerable<Attachment>> FetchAttachmentsAsync(
            string clientSecret, 
            string applicationId, 
            string tenantId, 
            string messageId, 
            string userPrincipalName,
            CancellationToken cancellationToken);
        Task<GraphServiceClient> GetAuthenticatedGraphClient(
            string clientSecret, 
            string applicationId, 
            string tenantId);
    }
}