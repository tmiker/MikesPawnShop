using Microsoft.EntityFrameworkCore;
using Products.Write.Infrastructure.Data;

namespace Products.Write.Infrastructure.DataAccess
{
    public class EventStoreDbContext : DbContext
    {
        public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options) : base(options)
        { }

        public DbSet<EventRecord> EventRecords { get; set; }
        public DbSet<OutboxRecord> OutboxRecords { get; set; }
        public DbSet<SnapshotRecord> SnapshotRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
