using Microsoft.EntityFrameworkCore;
using IncidentResponseAPI.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

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

            // Define composite key for IncidentEventModel
            modelBuilder.Entity<IncidentEventModel>()
                .HasKey(ie => new { ie.IncidentId, ie.EventId });

            modelBuilder.Entity<AttachmentModel>()
                .HasOne(a => a.Event)
                .WithMany(e => e.Attachments)
                .HasForeignKey(a => a.EventId)
                .OnDelete(DeleteBehavior.Cascade); //Optional

        }
        public DbSet<IncidentResponseAPI.Models.RecommendationsModel> RecommendationsModel { get; set; } = default!;
    }
}
