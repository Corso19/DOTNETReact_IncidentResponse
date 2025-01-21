using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace IncidentResponseAPI.Helpers;

public class IncidentHub : Hub
{
     public async Task NotifyNewIncidentCreated(int incidentId)
     {
         await Clients.All.SendAsync("ReceivedIncident", incidentId);
     }
}