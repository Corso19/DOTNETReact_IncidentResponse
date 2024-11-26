namespace IncidentResponseAPI.Dtos
{
    public class EventsDto
    {
        public int EventId { get; set; }
        public int SensorId { get; set; }
        //TODO - Change EventDataJson to separate fields <strings>
        //public string EventDataJson { get; set; }
        public string TypeName { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; }
        public bool isProcessed { get; set; }
    }
}

