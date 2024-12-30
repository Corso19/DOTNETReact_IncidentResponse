using Microsoft.Graph.Models;

namespace IncidentResponseAPI.Services.Interfaces
{
    public interface IGraphAuthService
    {
        /// <summary>
        /// Fetches all emails from the user's inbox.
        /// </summary>
        /// <param name="userId">The ID of the user whose emails should be fetched.</param>
        /// <returns>A collection of emails (Message objects).</returns>
        Task<IEnumerable<Message>> FetchEmailsAsync(string userId);

        /// <summary>
        /// Fetches the content of a specific email message.
        /// </summary>
        /// <param name="userId">The ID of the user who owns the email.</param>
        /// <param name="messageId">The ID of the email message to fetch.</param>
        /// <returns>A Message object containing the email's details.</returns>
        Task<Message> FetchMessageContentAsync(string userId, string messageId);

        /// <summary>
        /// Fetches all attachments for a specific email message.
        /// </summary>
        /// <param name="userId">The ID of the user who owns the email.</param>
        /// <param name="messageId">The ID of the email message to fetch attachments for.</param>
        /// <returns>A collection of attachments.</returns>
        Task<IEnumerable<Attachment>> FetchAttachmentsAsync(string userId, string messageId);
    }
}
