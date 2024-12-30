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

        public async Task<Dictionary<string, IEnumerable<Message>>> FetchEmailsForAllUsersAsync(string clientSecret,
            string applicationId, string tenantId, DateTime? lastProcessedTime)
        {
            _logger.LogInformation("Fetching emails for all users");

            try
            {
                var graphClient = await GetAuthenticatedGraphClient(clientSecret, applicationId, tenantId);

                // Get all users in the organization
                var users = await graphClient.Users.GetAsync();

                var allEmails = new Dictionary<string, IEnumerable<Message>>();

                foreach (var user in users.Value)
                {
                    _logger.LogInformation("Fetching emails for user: {UserPrincipalName}", user.UserPrincipalName);

                    try
                    {
                        // Build filter string for new emails
                        string filterString = lastProcessedTime.HasValue
                            ? $"receivedDateTime gt {lastProcessedTime.Value.ToString("o")}"
                            : null;

                        // Fetch messages directly with filter
                        var messages = await graphClient
                            .Users[user.Id]
                            .MailFolders["Inbox"]
                            .Messages
                            .GetAsync(requestConfiguration =>
                            {
                                if (!string.IsNullOrEmpty(filterString))
                                {
                                    requestConfiguration.QueryParameters.Filter = filterString;
                                }
                            });

                        if (messages != null && messages.Value.Any())
                        {
                            allEmails[user.UserPrincipalName] = messages.Value;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error fetching emails for user: {UserPrincipalName}",
                            user.UserPrincipalName);
                        continue;
                    }
                }

                return allEmails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching emails for all users");
                throw;
            }
        }

        
        public async Task<Message> FetchMessageContentAsync(string clientSecret, string applicationId, string tenantId, string messageId)
        {
            var graphClient = await _graphAuthProvider.GetAuthenticatedGraphClient(clientSecret, applicationId, tenantId);
            return await graphClient.Me.Messages[messageId].GetAsync();
        }

        // public async Task<IEnumerable<Attachment>> FetchAttachmentsAsync(string clientSecret, string applicationId, string tenantId, string messageId)
        // {
        //     var graphClient = await _graphAuthProvider.GetAuthenticatedGraphClient(clientSecret, applicationId, tenantId);
        //     return (await graphClient.Me.Messages[messageId].Attachments.GetAsync()).Value;
        // }
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