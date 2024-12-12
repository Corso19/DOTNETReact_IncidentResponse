using System.ComponentModel.DataAnnotations;

namespace IncidentResponseAPI.Models;

public class AttachmentModel
{
    [Key]
    public int AttachmentId { get; set; }
    public string Name { get; set; }
    public int Size { get; set; }
    public byte[] Content { get; set; }
    public int EventId { get; set; }
    //public EventsModel Event { get; set; }    
    
}