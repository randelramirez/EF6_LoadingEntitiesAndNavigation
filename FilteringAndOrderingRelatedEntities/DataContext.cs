using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilteringAndOrderingRelatedEntities
{
    public class DataContext : DbContext
    {
        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>()
                .HasKey(r => r.Id)
                .HasRequired(r => r.Hotel)
                .WithMany(r => r.Rooms)
                .HasForeignKey(r => r.HotelId);

            modelBuilder.Entity<Hotel>()
                .HasKey(h => h.Id);

            modelBuilder.Entity<Room>()
                .HasMany(r => r.Reservations)
                .WithRequired(r => r.Room)
                .HasForeignKey(r => r.RoomId);
               
        }
    }
}
