using System.Data.Entity;

namespace QueryingInMemoryEntities
{
    public class DataContext : DbContext
    {
        public DbSet<Club> Clubs { get; set; }
    }
}
