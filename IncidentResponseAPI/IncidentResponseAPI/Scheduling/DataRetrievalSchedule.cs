// using Quartz;
// using Quartz.Impl;
// using System.Threading.Tasks;
// using IncidentResponseAPI.Models;
//
// namespace IncidentResponseAPI.Scheduling;
//
// public class DataRetrievalSchedule
// {
//     private readonly IScheduler _scheduler;
//     
//     public DataRetrievalSchedule()
//     {
//         _scheduler = new StdSchedulerFactory().GetScheduler().Result;
//     }
//
//     public async Task ScheduleDataRetrieval(SensorsModel sensors)
//     {
//         var job = JobBuilder.Create<DataRetrievalJob>
//     }
// }