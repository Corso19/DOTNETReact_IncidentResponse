using IncidentResponseAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IncidentResponseAPI.Repositories.Implementations;

public class AttachmentRepository : IAttachmentRepository
{
    private readonly IncidentResponseContext _context;
    
    public AttachmentRepository(IncidentResponseContext context)
    {
        _context = context;
    }
    
    //Get all attachments for specific event by ID
    public async Task<IEnumerable<AttachmentModel>> GetAttachmentsByEventIdAsync(int eventId)
    {
        return await _context.Attachments
            .Where(a => a.EventId == eventId)
            .ToListAsync();
    }
    
    //Get a single attachment by ID
    public async Task<AttachmentModel> GetAttachmentByIdAsync(int attachmentId)
    {
        return await _context.Attachments.FirstOrDefaultAsync(a => a.AttachmentId == attachmentId);
    }
    
    //Add a new attachment 
    public async Task AddAttachmentAsync(AttachmentModel attachmentModel)
    {
        _context.Attachments.Add(attachmentModel);
        await _context.SaveChangesAsync();
    }
    
    //Update an attachment
    public async Task UpdateAttachmentAsync(AttachmentModel attachmentModel)
    {
        _context.Entry(attachmentModel).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    
    //Delete an attachment by ID
    public async Task DeleteAttachmentAsync(int attachmentId)
    {
        var attachmentModel = await _context.Attachments.FindAsync(attachmentId);
        if (attachmentModel != null)
        {
            _context.Attachments.Remove(attachmentModel);
            await _context.SaveChangesAsync();
        }
    }
}