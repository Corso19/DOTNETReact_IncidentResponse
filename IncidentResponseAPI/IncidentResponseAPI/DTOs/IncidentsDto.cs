namespace IncidentResponseAPI.Dtos
{
    public class IncidentsDto
    {
        public int IncidentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DetectedAt { get; set; }
        public string Status { get; set; }
    }
}
