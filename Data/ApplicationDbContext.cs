using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolSafetySystem.Models;

namespace SchoolSafetySystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Incident> Incidents => Set<Incident>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<IncidentCategory> IncidentCategories => Set<IncidentCategory>();
        public DbSet<IncidentDetail> IncidentDetails => Set<IncidentDetail>();
        public DbSet<IncidentCategoryMap> IncidentCategoryMaps => Set<IncidentCategoryMap>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IncidentCategoryMap>()
                .HasKey(x => new { x.IncidentId, x.IncidentCategoryId });
        }
    }
}
