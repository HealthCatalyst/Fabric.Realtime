using Fabric.Realtime.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Fabric.Realtime.Data.Stores
{
    public class RealtimeContext : DbContext
    {
        public RealtimeContext(DbContextOptions<RealtimeContext> options)
            : base(options)
        {
        }

        public DbSet<HL7Message> HL7Messages { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<ForwardingHistory> ForwardingHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var realtimeModelBuilder = new RealtimeModelBuilder();
            realtimeModelBuilder.BuildModel(builder);
        }

    }

}
