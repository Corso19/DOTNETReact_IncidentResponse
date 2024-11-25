namespace IncidentResponseAPI.Dtos
{
    public class RecommendationsDto
    {
        public int RecommendationId { get; set; }
        public int IncidentId { get; set; }
        public string Recommendation { get; set; }
        public bool isCompleted { get; set; }
    }
}
