using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using IncidentResponseAPI.Helpers;

namespace IncidentResponseAPI.Models
{
    public class SensorsModel
    {
        [Key]
        public int SensorId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string SensorName { get; set; }
        [Required]
        public string Type { get; set; }
        // [Required]
        // [RegularExpression(@"^[0-9a-fA-F-]{36}$", ErrorMessage = "Invalid TenantId format")]
        // public string TenantId { get; set; }
        // [Required]
        // [RegularExpression(@"^[0-9a-fA-F-]{36}$", ErrorMessage = "Invalid ApplicationId format")]
        // public string ApplicationId { get; set; }
        // [Required(ErrorMessage = "Client secret is required.")]
        // [StringLength(100, ErrorMessage = "Client secret must not exceed 100 characters.")]
        // public string ClientSecret { get; set; }
        [Required]
        [ValidConfiguration]
        public string Configuration { get; set; }
        public bool isEnabled { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastRunAt { get; set; }
        public DateTime? NextRunAfter { get; set; }//for scheduling
        [StringLength(1000, ErrorMessage = "LastError cannot exceed 1000 characters.")]
        public string LastError { get; set; } //Error message from the last run, if any
        [Range(1, 1440, ErrorMessage = "Sensor number must be between 1 and 1440 minutes.")]
        public int RetrievalInterval { get; set; }
        [StringLength(4000, ErrorMessage = "LastEventMarker must not exceed 4000 characters.")]
        public string LastEventMarker { get; set; } //Marker for the last event processed
    }
}
