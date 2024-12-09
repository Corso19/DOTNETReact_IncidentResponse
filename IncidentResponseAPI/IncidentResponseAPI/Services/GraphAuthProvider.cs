using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace IncidentResponseAPI.Services
{
    // This class handles the authentication with Microsoft Graph using Microsoft.Identity.Web
    public class GraphAuthProvider
    {
        private readonly string _tenantId;
        private readonly string _applicationId;
        private readonly string _clientSecret;

        public GraphAuthProvider()
        {
            _clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
            _tenantId = Environment.GetEnvironmentVariable("TENANT_ID");
            _applicationId = Environment.GetEnvironmentVariable("APPLICATION_ID");
        }

        public async Task<GraphServiceClient> GetAuthenticatedGraphClient()
        {
            var clientSecretCredential = new ClientSecretCredential(_tenantId, _applicationId, _clientSecret);
            var graphServiceClient = new GraphServiceClient(clientSecretCredential, new[] { "https://graph.microsoft.com/.default" });

            return graphServiceClient;
        }
    }

    // This class contains the test logic to interact with Microsoft Graph
    // public class GraphTest
    // {
    //     public async Task TestGraphConnection()
    //     {
    //         var graphAuthProvider = new GraphAuthProvider();
    //         var graphClient = await graphAuthProvider.GetAuthenticatedGraphClient();
    //
    //         try
    //         {
    //             // Fetch the users from Microsoft Graph
    //             var users = await graphClient
    //                 .Users
    //                 .GetAsync();
    //
    //             // Loop through and print user details
    //             foreach (var user in users.Value)
    //             {
    //                 Console.WriteLine($"User: {user.DisplayName}, Email: {user.Mail}");
    //             }
    //         }
    //         catch (Exception ex)
    //         {
    //             Console.WriteLine($"Error fetching users: {ex.Message}");
    //         }
    //     }
    // }
    
    public class GraphTest
    {
        private readonly GraphAuthProvider _graphAuthProvider;

        public GraphTest()
        {
            _graphAuthProvider = new GraphAuthProvider();
        }

        public async Task TestFetchUsers()
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
            }
        }

        public async Task TestFetchEmails()
        {
            var graphClient = await _graphAuthProvider.GetAuthenticatedGraphClient();

            try
            {
                // Fetch users
                var users = await graphClient.Users.GetAsync();

                if (users.Value.Any())
                {
                    var firstUser = users.Value.First();

                    // Fetch emails from the inbox of the first user
                    var messages = await graphClient.Users[firstUser.Id].MailFolders["Inbox"].Messages.GetAsync();

                    Console.WriteLine($"Emails in the inbox of {firstUser.DisplayName}:");
                    foreach (var message in messages.Value)
                    {
                        Console.WriteLine($"Message ID: {message.Id}, Subject: {message.Subject}, From: {message.From?.EmailAddress?.Address}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching emails: {ex.Message}");
            }
        }

        public async Task TestFetchMessageContent()
        {
            var graphClient = await _graphAuthProvider.GetAuthenticatedGraphClient();

            try
            {
                // Fetch users
                var users = await graphClient.Users.GetAsync();

                if (users.Value.Any())
                {
                    var firstUser = users.Value.First();

                    // Fetch emails
                    var messages = await graphClient.Users[firstUser.Id].MailFolders["Inbox"].Messages.GetAsync();

                    if (messages.Value.Any())
                    {
                        var firstMessage = messages.Value.First();

                        // Fetch message content
                        var detailedMessage = await graphClient.Users[firstUser.Id]
                            .Messages[firstMessage.Id]
                            .GetAsync();

                        Console.WriteLine($"Message Content: {detailedMessage.Body?.Content}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching message content: {ex.Message}");
            }
        }

        public async Task TestFetchAttachments()
        {
            var graphClient = await _graphAuthProvider.GetAuthenticatedGraphClient();

            try
            {
                // Fetch users
                var users = await graphClient.Users.GetAsync();

                if (users.Value.Any())
                {
                    var firstUser = users.Value.First();

                    // Fetch emails
                    var messages = await graphClient.Users[firstUser.Id].MailFolders["Inbox"].Messages.GetAsync();

                    if (messages.Value.Any())
                    {
                        var firstMessage = messages.Value.First();

                        // Fetch attachments
                        var attachments = await graphClient.Users[firstUser.Id]
                            .Messages[firstMessage.Id]
                            .Attachments
                            .GetAsync();

                        Console.WriteLine($"Attachments for the message '{firstMessage.Subject}':");
                        foreach (var attachment in attachments.Value)
                        {
                            Console.WriteLine($"Attachment Name: {attachment.Name}, Size: {attachment.Size}");

                            if (attachment is FileAttachment fileAttachment)
                            {
                                Console.WriteLine($"Attachment Content: {fileAttachment.ContentBytes.Length} bytes");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching attachments: {ex.Message}");
            }
        }
    }
}