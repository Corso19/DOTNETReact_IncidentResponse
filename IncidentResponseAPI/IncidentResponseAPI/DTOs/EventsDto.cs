namespace IncidentResponseAPI.Dtos
{
    public class EventsDto
    {
        public int EventId { get; set; }
        public int SensorId { get; set; }
        public string EventDataJson { get; set; }
        public DateTime Timestamp { get; set; }
        public bool isProcessed { get; set; }
    }
}

