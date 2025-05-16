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

        //Email related operations
        public async Task<Dictionary<string, List<Message>>> FetchEmailsForAllUsersAsync(
            string clientSecret, string applicationId, string tenantId, DateTime? lastProcessedTime,
            CancellationToken cancellationToken)
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

        public async Task<Message> FetchMessageContentAsync(string clientSecret, string applicationId, string tenantId,
            string messageId)
        {
            var graphClient =
                await _graphAuthProvider.GetAuthenticatedGraphClient(clientSecret, applicationId, tenantId);
            return await graphClient.Me.Messages[messageId].GetAsync();
        }

        public async Task<IEnumerable<Attachment>> FetchAttachmentsAsync(string clientSecret, string applicationId,
            string tenantId, string messageId, string userPrincipalName, CancellationToken cancellationToken)
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
                _logger.LogError(ex, "Error occurred while fetching attachments for message ID: {MessageId}",
                    messageId);
                throw;
            }
        }

        //Teams related operations

        public async Task<List<ChatMessage>> FetchTeamsMessagesAsync(string clientSecret, string applicationId,
            string tenantId, DateTime? since,
            CancellationToken cancellationToken)
        {
            try
            {
                var graphClient = await GetAuthenticatedGraphClient(clientSecret, applicationId, tenantId);

                //Creating filter for fetching messages since specified time
                var filter = since.HasValue
                    ? $"createdDateTime gt {since.Value.ToUniversalTime():yyyy-MM-ddTHH:mm:ssZ}"
                    : null;

                _logger.LogInformation("Fetching Teams messages with filter: {Filter}", filter);

                //Getting all chats from Teams
                var chats = await graphClient.Chats.GetAsync(cancellationToken: cancellationToken);

                var allMessages = new List<ChatMessage>();
                //For each chat, get its messages
                if (chats?.Value != null)
                {
                    foreach (var chat in chats.Value)
                    {
                        var chatMessages = await graphClient.Chats[chat.Id].Messages
                            .GetAsync(requestConfiguration =>
                                {
                                    if (!string.IsNullOrEmpty(filter))
                                    {
                                        requestConfiguration.QueryParameters.Filter = filter;
                                        requestConfiguration.QueryParameters.Orderby = new[] { "createdDateTime" };
                                    }
                                }
                                , cancellationToken);

                        if (chatMessages?.Value != null)
                        {
                            allMessages.AddRange(chatMessages.Value);
                        }
                    }
                }

                return allMessages
                    .OrderBy(m => m.CreatedDateTime)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Teams messages.");
                throw;
            }
        }

        public async Task<byte[]> FetchTeamsAttachmentAsync(string clientSecret, string applicationId, string tenantId,
            string messageId,
            string attachmentId, CancellationToken cancellationToken)
        {
            try
            {
                var graphClient = await GetAuthenticatedGraphClient(clientSecret, applicationId, tenantId);
                var chats = await graphClient.Chats.GetAsync(cancellationToken: cancellationToken);

                foreach (var chat in chats.Value)
                {
                    try
                    {
                        var message = await graphClient.Chats[chat.Id].Messages[messageId]
                            .GetAsync(cancellationToken: cancellationToken);

                        if (message != null && message.Attachments != null)
                        {
                            var attachment = message.Attachments.FirstOrDefault(a => a.Id == attachmentId);
                            //Handling different attachment types
                            if (!string.IsNullOrEmpty(attachment.ContentUrl))
                            {
                                //For hosted content, download from the URL
                                using var httpClient = new HttpClient();
                                return await httpClient.GetByteArrayAsync(new Uri(attachment.ContentUrl),
                                    cancellationToken);
                            }
                            else if (attachment.Content != null)
                            {
                                //for inline content, return the byte array directly
                                return Convert.FromBase64String(attachment.Content);
                            }
                        }
                    }
                    catch (ServiceException)
                    {
                        _logger.LogInformation("Message not found in chat {ChatId} for message ID: {MessageId}",
                            chat.Id, messageId);
                        continue;
                    }
                }

                throw new KeyNotFoundException(
                    $"No attachment found for message ID: {messageId} and attachment ID: {attachmentId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Teams attachment for message ID: {MessageId}",
                    messageId);
                throw;
            }
        }

        //General operations
        public async Task<GraphServiceClient> GetAuthenticatedGraphClient(string clientSecret, string applicationId,
            string tenantId)
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