using Microsoft.EntityFrameworkCore;


namespace TransportStore.Models
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

        public DbSet<Transport> Transports => Set<Transport>();
    }
}
