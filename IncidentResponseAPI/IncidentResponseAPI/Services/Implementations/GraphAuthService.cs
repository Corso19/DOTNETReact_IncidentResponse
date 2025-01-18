using Azure.Identity;
using Microsoft.Graph.Models;
using IncidentResponseAPI.Services.Interfaces;
using Microsoft.Graph;
using System.Threading;
using System.Threading.Tasks;

namespace IncidentResponseAPI.Services.Implementations
{
    public class GraphAuthService : IGraphAuthService
    {
        private readonly GraphAuthProvider _graphAuthProvider;
        private readonly ILogger<GraphAuthService> _logger;

        public GraphAuthService(GraphAuthProvider graphAuthProvider, ILogger<GraphAuthService> logger)
        {
            _graphAuthProvider = graphAuthProvider;
            _logger = logger;
        }
        
        public async Task<Dictionary<string, List<Message>>> FetchEmailsForAllUsersAsync(
            string clientSecret, string applicationId, string tenantId, DateTime? lastProcessedTime, CancellationToken cancellationToken)
        {
            var graphClient = await GetAuthenticatedGraphClient(clientSecret, applicationId, tenantId);
            var emailsByUser = new Dictionary<string, List<Message>>();
    
            var users = await graphClient.Users.GetAsync(cancellationToken: cancellationToken);
            foreach (var user in users.Value)
            {
                var userId = user.Id;

                // Ensure strict greater than comparison
                var filter = lastProcessedTime.HasValue 
                    ? $"receivedDateTime gt {lastProcessedTime.Value.ToUniversalTime():yyyy-MM-ddTHH:mm:ssZ}" 
                    : null;

                _logger.LogInformation("Fetching emails with filter: {Filter}", filter);

                var messages = await graphClient.Users[userId]
                    .MailFolders["Inbox"]
                    .Messages
                    .GetAsync(requestConfiguration =>
                    {
                        if (!string.IsNullOrEmpty(filter))
                        {
                            requestConfiguration.QueryParameters.Filter = filter;
                            requestConfiguration.QueryParameters.Orderby = new[] { "receivedDateTime" };
                        }
                    }, cancellationToken);

                if (messages?.Value != null)
                {
                    // Double-check timestamps to ensure no duplicates
                    var filteredMessages = messages.Value
                        .Where(m => !lastProcessedTime.HasValue || 
                                    m.ReceivedDateTime > lastProcessedTime.Value)
                        .OrderBy(m => m.ReceivedDateTime)
                        .ToList();

                    emailsByUser[userId] = filteredMessages;
            
                    _logger.LogInformation("Retrieved {Count} new messages for user {UserId}", 
                        filteredMessages.Count, userId);
                }
            }

            return emailsByUser;
        }

        public async Task<Message> FetchMessageContentAsync(string clientSecret, string applicationId, string tenantId, string messageId)
        {
            var graphClient = await _graphAuthProvider.GetAuthenticatedGraphClient(clientSecret, applicationId, tenantId);
            return await graphClient.Me.Messages[messageId].GetAsync();
        }

        public async Task<IEnumerable<Attachment>> FetchAttachmentsAsync(string clientSecret, string applicationId, string tenantId, string messageId, string userPrincipalName, CancellationToken cancellationToken)
        {
            try
            {
                // Authenticate using client credentials
                var graphClient = await GetAuthenticatedGraphClient(clientSecret, applicationId, tenantId);

                // Use the specific user's mailbox
                var attachments = await graphClient.Users[userPrincipalName]
                    .Messages[messageId]
                    .Attachments
                    .GetAsync(cancellationToken: cancellationToken);

                return attachments.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching attachments for message ID: {MessageId}", messageId);
                throw;
            }
        }

        public async Task<GraphServiceClient> GetAuthenticatedGraphClient(string clientSecret, string applicationId, string tenantId)
        {
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            var clientSecretCredential = new ClientSecretCredential(tenantId, applicationId, clientSecret, options);
            return new GraphServiceClient(clientSecretCredential);
        }
    }
}