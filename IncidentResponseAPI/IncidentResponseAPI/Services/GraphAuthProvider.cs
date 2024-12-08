﻿using Azure.Identity;
using Microsoft.Graph;

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
    public class GraphTest
    {
        public async Task TestGraphConnection()
        {
            var graphAuthProvider = new GraphAuthProvider();
            var graphClient = await graphAuthProvider.GetAuthenticatedGraphClient();

            try
            {
                // Fetch the users from Microsoft Graph
                var users = await graphClient
                    .Users
                    .GetAsync();

                // Loop through and print user details
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
    }
}