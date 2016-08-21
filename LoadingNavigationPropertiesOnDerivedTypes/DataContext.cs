using System.Data.Entity;

namespace LoadingNavigationPropertiesOnDerivedTypes
{
    public class DataContext : DbContext
    {
        public DbSet<Foreman> Foremen { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Phone> Phones { get; set; }

        public DbSet<Tradesman> Tradesmen { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Foreman>()
                .HasKey(f => f.Id)
                .HasRequired(f => f.JobSite)
                .WithMany(f => f.Foremen)
                .HasForeignKey(f => f.LocationId);

            modelBuilder.Entity<JobSite>()
                .HasKey(j => j.Id)
                .HasMany(j => j.Plumbers)
                .WithRequired(j => j.JobSite)
                .HasForeignKey(j => j.LocationId);

            modelBuilder.Entity<Location>()
                .HasKey(l => l.Id)
                .HasRequired(l => l.Phone)
                .WithMany(l => l.Locations)
                .HasForeignKey(l => l.PhoneId);

        }
    }
}
