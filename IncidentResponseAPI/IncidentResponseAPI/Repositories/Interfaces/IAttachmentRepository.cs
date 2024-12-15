using IncidentResponseAPI.Models;

namespace IncidentResponseAPI.Repositories;

public interface IAttachmentRepository
{
    Task<IEnumerable<AttachmentModel>> GetAttachmentsByEventIdAsync(int eventId);
    Task<AttachmentModel> GetAttachmentByIdAsync(int attachmentId);
    Task AddAttachmentAsync(AttachmentModel attachmentModel);
    Task UpdateAttachmentAsync(AttachmentModel attachmentModel);
    Task DeleteAttachmentAsync(int attachmentId);
    
}