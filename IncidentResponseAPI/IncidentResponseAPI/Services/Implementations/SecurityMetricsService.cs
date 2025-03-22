namespace IncidentResponseAPI.Services.Implementations;

using Prometheus;

public class SecurityMetricsService
{
    //sensor metrics
    public readonly Counter SensorRuns;
    public readonly Counter SensorErrors;
    public readonly Gauge ActiveSensors;
    
    //Incident metrics
    public readonly Counter IncidentsDetected;
    public readonly Histogram EventProcessingTime;

    public SecurityMetricsService()
    {
        SensorRuns = Metrics.CreateCounter("ir_sensor_runs_total", "Number of sensor executions",
            new CounterConfiguration { LabelNames = new[] { "sensor_type", "sensor_name" } });
        
        SensorErrors = Metrics.CreateCounter("ir_sensor_errors_total", 
            "Number of sensor errors",
            new CounterConfiguration { LabelNames = new[] { "sensor_type", "sensor_name" } });

        ActiveSensors = Metrics.CreateGauge("ir_active_sensors",
            "Number of currently active sensors",
            new GaugeConfiguration { LabelNames = new[] { "sensor_type" }});
        
        IncidentsDetected = Metrics.CreateCounter("ir_incidents_detected_total",
            "Number of security incidents detected",
            new CounterConfiguration { LabelNames = new[] { "severity", "type" }});
            
        EventProcessingTime = Metrics.CreateHistogram("ir_event_processing_seconds",
            "Time taken to process events",
            new HistogramConfiguration { LabelNames = new[] { "event_type" }});
    }
}