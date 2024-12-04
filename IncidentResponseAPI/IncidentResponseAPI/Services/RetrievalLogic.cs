using IncidentResponseAPI.Models;

namespace IncidentResponseAPI.Services;

public class RetrievalLogic
{
    public async Task<string> RetrieveDataFromSensor(SensorsModel sensor)
    {
        await Task.Delay(1000);
        return "data";
    }
}