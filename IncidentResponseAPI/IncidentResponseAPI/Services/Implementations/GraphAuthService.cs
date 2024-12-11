using Microsoft.Graph.Models;
using IncidentResponseAPI.Services.Interfaces;

namespace IncidentResponseAPI.Services.Implementations
{
    public class GraphAuthService : IGraphAuthService
    {
        private readonly GraphAuthProvider _graphAuthProvider;

        public GraphAuthService()
        {
            _graphAuthProvider = new GraphAuthProvider();
        }

        public async Task<IEnumerable<User>> FetchUsersAsync()
        {
            var graphClient = await _graphAuthProvider.GetAuthenticatedGraphClient();

            try
            {
                // Fetch users
                var users = await graphClient.Users.GetAsync();
                Console.WriteLine("Users:");

                foreach (var user in users.Value)
                {
                    Console.WriteLine($"User: {user.DisplayName}, Email: {user.Mail}");
                }

                return users.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                return Enumerable.Empty<User>();
            }
        }

        public async Task<IEnumerable<Message>> FetchEmailsAsync(string userId)
        {
            var graphClient = await _graphAuthProvider.GetAuthenticatedGraphClient();

            try
            {
                // Fetch emails from the inbox of the specified user
                var messages = await graphClient.Users[userId].MailFolders["Inbox"].Messages.GetAsync();
                Console.WriteLine("Emails:");

                foreach (var message in messages.Value)
                {
                    Console.WriteLine($"Message ID: {message.Id}, Subject: {message.Subject}, From: {message.From?.EmailAddress?.Address}");
                }

                return messages.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching emails: {ex.Message}");
                return Enumerable.Empty<Message>();
            }
        }

        public async Task<Message> FetchMessageContentAsync(string userId, string messageId)
        {
            var graphClient = await _graphAuthProvider.GetAuthenticatedGraphClient();

            try
            {
                // Fetch message content
                var detailedMessage = await graphClient.Users[userId].Messages[messageId].GetAsync();
                Console.WriteLine($"Message Content: {detailedMessage.Body?.Content}");
                return detailedMessage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching message content: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<Attachment>> FetchAttachmentsAsync(string userId, string messageId)
        {
            var graphClient = await _graphAuthProvider.GetAuthenticatedGraphClient();

            try
            {
                // Fetch attachments
                var attachments = await graphClient.Users[userId].Messages[messageId].Attachments.GetAsync();
                Console.WriteLine("Attachments:");

                foreach (var attachment in attachments.Value)
                {
                    Console.WriteLine($"Attachment Name: {attachment.Name}, Size: {attachment.Size}");

                    if (attachment is FileAttachment fileAttachment)
                    {
                        Console.WriteLine($"Attachment Content: {fileAttachment.ContentBytes.Length} bytes");
                    }
                }

                return attachments.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching attachments: {ex.Message}");
                return Enumerable.Empty<Attachment>();
            }
        }
    }
}