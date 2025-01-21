using IncidentResponseAPI.Models;

namespace IncidentResponseAPI.Constants;

public static class RecommendationMetadata
{
    private static readonly Dictionary<IncidentType, string> Recommendations = new()
    {
        { IncidentType.UnusualEmailVolume, 
            "1. Monitor sender's email patterns\n2. Check for automated sending\n3. Implement rate limiting" },
        { IncidentType.SuspiciousAttachment, 
            "1. Quarantine attachment\n2. Scan in sandbox\n3. Update blocking rules" },
        { IncidentType.ExternalSender, 
            "1. Verify sender identity\n2. Check domain reputation\n3. Update allowed senders" },
        { IncidentType.RepeatedEventPattern, 
            "1. Analyze pattern frequency\n2. Identify source\n3. Block if malicious" }
    };

    public static string GetRecommendation(IncidentType type) =>
        Recommendations.TryGetValue(type, out var recommendation) 
            ? recommendation 
            : "Investigate and document the incident";
}