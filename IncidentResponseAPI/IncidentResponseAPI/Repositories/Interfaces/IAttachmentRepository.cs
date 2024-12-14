using IncidentResponseAPI.Models;

namespace IncidentResponseAPI.Repositories;

public interface IAttachmentRepository
{
    Task<IEnumerable<AttachmentModel>> GetAttachmentsByEventIdAsync(int eventId);
    Task<AttachmentModel> GetAttachmentByIdAsync(int attachmentId);
    Task AddAsync(AttachmentModel attachmentModel);
    Task DeleteAsync(int attachmentId);
    
}