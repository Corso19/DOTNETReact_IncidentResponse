namespace IncidentResponseAPI.Models
{
    public class IncidentEventModel
    {

        public int IncidentId { get; set; }
        public IncidentsModel Incident { get; set; }


        public int EventId { get; set; }
        public EventsModel Event { get; set; }


    }
}
