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
        public DbSet<RecommendationsModel> Recommendations { get; set; }
        public DbSet<AttachmentModel> Attachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key for IncidentEventModel
            modelBuilder.Entity<IncidentEventModel>()
                .HasKey(ie => new { ie.IncidentId, ie.EventId });

            // Relationship for Attachments
            modelBuilder.Entity<AttachmentModel>()
                .HasOne(a => a.Event)
                .WithMany(e => e.Attachments)
                .HasForeignKey(a => a.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // SensorsModel Constraints
            modelBuilder.Entity<SensorsModel>()
                .HasCheckConstraint("CK_Sensors_RetrievalInterval", "[RetrievalInterval] BETWEEN 1 AND 1440")
                .Property(s => s.Configuration)
                .IsRequired(); // Ensures non-null Configuration

            // Add more model-specific configurations as needed.
        }
    }
}