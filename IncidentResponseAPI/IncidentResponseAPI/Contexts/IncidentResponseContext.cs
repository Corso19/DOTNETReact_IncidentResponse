using Microsoft.EntityFrameworkCore;

namespace IncidentResponseAPI.Models
{
    public class IncidentResponseContext : DbContext
    {
        public IncidentResponseContext(DbContextOptions<IncidentResponseContext> options)
            : base(options)
        {
        }

        public DbSet<IncidentEventModel> IncidentEvents { get; set; }
        public DbSet<IncidentsModel> Incidents { get; set; }
        public DbSet<EventsModel> Events { get; set; }
        public DbSet<SensorsModel> Sensors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define composite key for IncidentEventModel
            modelBuilder.Entity<IncidentEventModel>()
                .HasKey(ie => new { ie.IncidentId, ie.EventId });

            //// Seed data (optional)
            //modelBuilder.Entity<SensorsModel>().HasData(
            //    new SensorsModel { SensorId = 1, SensorName = "Sensor1", Type = "Type1", ConfigurationJson = "{}", isEnabled = true, CreatedAd = DateTime.Now }
            //);

            //modelBuilder.Entity<IncidentsModel>().HasData(
            //    new IncidentsModel { IncidentId = 1, Title = "Incident1", Description = "Description1", DetectedAt = DateTime.Now, Status = "Open" }
            //);

            //modelBuilder.Entity<EventsModel>().HasData(
            //    new EventsModel { EventId = 1, SensorId = 1, EventDataJson = "{}", Timestamp = DateTime.Now, isProcessed = false }
            //);

            //modelBuilder.Entity<IncidentEventModel>().HasData(
            //    new IncidentEventModel { IncidentId = 1, EventId = 1 }
            //);
        }
    }
}
