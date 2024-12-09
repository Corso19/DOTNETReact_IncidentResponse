using System.ComponentModel.DataAnnotations;

namespace IncidentResponseAPI.Models
{
    public class EventsModel
    {
        [Key]
        public int EventId { get; set; } // This will be recognized as the primary key
        public int SensorId { get; set; }
        public SensorsModel Sensor { get; set; }
        public string TypeName { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public bool isProcessed { get; set; } = false;
    }
}
