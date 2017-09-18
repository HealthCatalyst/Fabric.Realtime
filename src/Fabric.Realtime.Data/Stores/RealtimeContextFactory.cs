using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Fabric.Realtime.Data.Stores
{
    public class RealtimeContextFactory : IDesignTimeDbContextFactory<RealtimeContext>
    {
        public RealtimeContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<RealtimeContext>();
            // TODO Move this to an external configuration
            builder.UseSqlServer("Server=(local);Database=FabricRealtime;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new RealtimeContext(builder.Options);
        }
    }
}