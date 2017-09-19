namespace Fabric.Realtime.Data.Stores
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    /// <summary>
    /// Factory class for realtime database context creation.
    /// </summary>
    public class RealtimeContextFactory : IDesignTimeDbContextFactory<RealtimeContext>
    {
        /// <summary>
        /// Creates a realtime database context.
        /// </summary>
        /// <param name="args">
        /// The argument array.
        /// </param>
        /// <returns>
        /// A <see cref="RealtimeContext"/> instance.
        /// </returns>
        public RealtimeContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<RealtimeContext>();
            // TODO Move this to an external configuration
            builder.UseSqlServer("Server=(local);Database=FabricRealtime;Trusted_Connection=True;MultipleActiveResultSets=true");
            return new RealtimeContext(builder.Options);
        }
    }
}