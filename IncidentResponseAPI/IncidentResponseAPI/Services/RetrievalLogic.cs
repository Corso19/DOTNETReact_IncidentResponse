using IncidentResponseAPI.Models;

namespace IncidentResponseAPI.Services;
//TODO - Completion of feature after email service link with backend application
public class RetrievalLogic
{
    public async Task<string> RetrieveDataFromSensor(SensorsModel sensor)
    {
        await Task.Delay(1000);
        return "data";
    }
}