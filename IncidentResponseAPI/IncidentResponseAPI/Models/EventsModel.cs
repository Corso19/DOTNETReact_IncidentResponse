using System.ComponentModel.DataAnnotations;

namespace IncidentResponseAPI.Models
{
    public class EventsModel
    {
        [Key]
        public int EventId { get; set; }
        public int SensorId { get; set; }
        public SensorsModel Sensor { get; set; }
        public string TypeName { get; set; } = "Email";
        public string Subject { get; set; }
        public string Sender { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public bool isProcessed { get; set; } = false;
        public string MessageId { get; set; }   
    }
}
