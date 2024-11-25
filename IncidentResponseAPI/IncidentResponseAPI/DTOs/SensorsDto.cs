namespace IncidentResponseAPI.Dtos
{
    public class SensorsDto
    {
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public string Type { get; set; }
        public string ConfigurationJson { get; set; }
        public bool isEnabled { get; set; }
        public DateTime CreatedAd { get; set; }
        public DateTime? LastRunAt { get; set; }
    }
}


