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

        public async Task<GraphServiceClient> GetAuthenticatedGraphClient(string clientSecret, string applicationId, string tenantId)
        {
            // var clientSecretCredential = new ClientSecretCredential(_tenantId, _applicationId, _clientSecret);
            // var graphServiceClient = new GraphServiceClient(clientSecretCredential, new[] { "https://graph.microsoft.com/.default" });
            //
            // return graphServiceClient;

            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };
            
            var clientSecretCredential = new ClientSecretCredential(tenantId, applicationId, clientSecret, options);
            return new GraphServiceClient(clientSecretCredential);
        }
    }
}