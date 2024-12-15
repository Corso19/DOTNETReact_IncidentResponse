using Microsoft.Graph.Models;
using IncidentResponseAPI.Services.Interfaces;

namespace IncidentResponseAPI.Services.Implementations
{
    public class GraphAuthService : IGraphAuthService
    {
        private readonly GraphAuthProvider _graphAuthProvider;

        public GraphAuthService(GraphAuthProvider graphAuthProvider)
        {
            _graphAuthProvider = graphAuthProvider;
        }

        public async Task<IEnumerable<Message>> FetchEmailsAsync(string userId)
        {
            var graphClient = await _graphAuthProvider.GetAuthenticatedGraphClient();
            return (await graphClient.Users[userId].MailFolders["Inbox"].Messages.GetAsync()).Value;
        }

        public async Task<Message> FetchMessageContentAsync(string userId, string messageId)
        {
            var graphClient = await _graphAuthProvider.GetAuthenticatedGraphClient();
            return await graphClient.Users[userId].Messages[messageId].GetAsync();
        }

        public async Task<IEnumerable<Attachment>> FetchAttachmentsAsync(string userId, string messageId)
        {
            var graphClient = await _graphAuthProvider.GetAuthenticatedGraphClient();
            return (await graphClient.Users[userId].Messages[messageId].Attachments.GetAsync()).Value;
        }
    }
}