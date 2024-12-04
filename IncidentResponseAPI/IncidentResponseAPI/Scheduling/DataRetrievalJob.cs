using Quartz;
using System;
using System.Formats.Asn1;
using System.Threading.Tasks;
using IncidentResponseAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
//TODO - Completion of feature after email service link with backend application
namespace IncidentResponseAPI.Scheduling;

public class DataRetrievalJob : IJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DataRetrievalJob> _logger;
    
    public DataRetrievalJob(IServiceProvider serviceProvider, ILogger<DataRetrievalJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var sensorId = context.JobDetail.JobDataMap.GetInt("SensorId");
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IncidentResponseContext>();
            var sensor = await dbContext.Sensors.FindAsync(sensorId);

            if (sensor != null && sensor.isEnabled)
            {
                //Perform data retrieval logic here
                _logger.LogInformation($"Data retrieval for sensor {sensor.SensorName} is complete.");
                
                //Placeholder for actual data retrieval logic
                //example: var data = await RetrieveDataFromSensor(sensor);
                
                sensor.LastRunAt = DateTime.Now;
                await dbContext.SaveChangesAsync();
            }
        }
    }
}