using Microsoft.EntityFrameworkCore;

namespace TransportStore.Domain.Models
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

        public DbSet<Transport> Transports => Set<Transport>();

        public DbSet<Review> Reviews => Set<Review>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transport>()
                .Property(t => t.PricePerHour)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Transport)
                .WithMany(t => t.Reviews)
                .HasForeignKey(r => r.TransportId);
        }
    }
}