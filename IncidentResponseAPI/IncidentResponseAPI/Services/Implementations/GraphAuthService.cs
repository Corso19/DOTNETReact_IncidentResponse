using Azure.Identity;
using Microsoft.Graph.Models;
using IncidentResponseAPI.Services.Interfaces;
using Microsoft.Graph;

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
            string clientSecret, string applicationId, string tenantId, DateTime? lastProcessedTime)
        {
            var graphClient = await GetAuthenticatedGraphClient(clientSecret, applicationId, tenantId);

            var emailsByUser = new Dictionary<string, List<Message>>();

            // Fetch all users
            var users = await graphClient.Users.GetAsync();
            foreach (var user in users.Value)
            {
                var userId = user.Id;

                // Create a filter string if a lastProcessedTime is provided
                var filter = lastProcessedTime.HasValue ? $"receivedDateTime gt {lastProcessedTime.Value:o}" : null;

                var messages = await graphClient.Users[userId]
                    .MailFolders["Inbox"]
                    .Messages
                    .GetAsync(requestConfiguration =>
                    {
                        if (!string.IsNullOrEmpty(filter))
                        {
                            requestConfiguration.QueryParameters.Filter = filter;
                        }
                    });

                if (messages?.Value != null)
                {
                    emailsByUser[userId] = messages.Value.ToList();
                }
            }

            return emailsByUser;
        }

        
        public async Task<Message> FetchMessageContentAsync(string clientSecret, string applicationId, string tenantId, string messageId)
        {
            var graphClient = await _graphAuthProvider.GetAuthenticatedGraphClient(clientSecret, applicationId, tenantId);
            return await graphClient.Me.Messages[messageId].GetAsync();
        }
        
        public async Task<IEnumerable<Attachment>> FetchAttachmentsAsync(string clientSecret, string applicationId, string tenantId, string messageId, string userPrincipalName)
        {
            try
            {
                // Authenticate using client credentials
                var graphClient = await GetAuthenticatedGraphClient(clientSecret, applicationId, tenantId);

                // Use the specific user's mailbox
                var attachments = await graphClient.Users[userPrincipalName]
                    .Messages[messageId]
                    .Attachments
                    .GetAsync();

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