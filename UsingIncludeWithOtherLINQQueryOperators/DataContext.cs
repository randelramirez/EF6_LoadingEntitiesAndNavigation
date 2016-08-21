using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingIncludeWithOtherLINQQueryOperators
{
    public class DataContext : DbContext
    {
        public DbSet<Club> Clubs { get; set; }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Club>()
                .HasKey(c => c.Id)
                .HasMany(c => c.Events)
                .WithRequired(c => c.Club)
                .HasForeignKey(c => c.ClubId);
        }

    }
}
