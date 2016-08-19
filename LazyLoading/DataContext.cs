using System.Data.Entity;

namespace LazyLoading
{
    public class DataContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerEmail> CustomerEmails { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.Id)
                .HasMany(c => c.CustomerEmails)
                .WithRequired(c => c.Customer)
                .HasForeignKey(c => c.CustomerId);

            modelBuilder.Entity<Customer>()
                .HasRequired(c => c.CustomerType)
                .WithMany(c => c.Customers)
                .HasForeignKey(c => c.CustomerTypeId);

            modelBuilder.Entity<CustomerEmail>().HasKey(c => c.Id);

            modelBuilder.Entity<CustomerType>().HasKey(c => c.Id);
        }

    }
}
