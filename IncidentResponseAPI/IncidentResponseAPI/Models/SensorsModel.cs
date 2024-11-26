using System.ComponentModel.DataAnnotations;

namespace IncidentResponseAPI.Models
{
    public class SensorsModel
    {
        [Key]
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public string Type { get; set; }
        //TODO - Change EventDataJson to separate fields <strings>: tenantId, applicationId, clientSecret
        /*public string ConfigurationJson { get; set; }*/
        public string TenantId { get; set; }
        public string ApplicationId { get; set; }
        public string ClientSecret { get; set; }
        public bool isEnabled { get; set; }
        public DateTime CreatedAd { get; set; } = DateTime.Now;
        public DateTime? LastRunAt { get; set; }
    }
}
