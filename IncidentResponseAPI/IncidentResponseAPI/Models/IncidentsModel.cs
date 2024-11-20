using System.ComponentModel.DataAnnotations;

namespace IncidentResponseAPI.Models
{
    public class IncidentsModel
    {
        [Key]
        public int IncidentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DetectedAt { get; set; } = DateTime.Now;
        public string Status { get; set; }
        public ICollection<IncidentEventModel> IncidentEvent { get; set; }

    }
}
