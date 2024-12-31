namespace IncidentResponseAPI.Dtos
{
    public class SensorDto
    {
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public string Type { get; set; }
        //public string ConfigurationJson { get; set; }
        // public string TenantId { get; set; }
        // public string ApplicationId { get; set; }
        // public string ClientSecret { get; set; } = string.Empty;
        public string Configuration { get; set; }
        public bool isEnabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastRunAt { get; set; }
        public DateTime? NextRunAfter { get; set; }
        public string LastError { get; set; }
        public int RetrievalInterval { get; set; }
        public string LastEventMarker { get; set; }
    }
}


