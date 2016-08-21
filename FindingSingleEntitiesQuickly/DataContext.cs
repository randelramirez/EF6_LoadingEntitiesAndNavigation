using System.Data.Entity;

namespace FindingSingleEntitiesQuickly
{
    public class DataContext : DbContext
    {
        public DbSet<Club> Clubs { get; set; }
    }
}
