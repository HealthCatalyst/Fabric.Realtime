namespace Fabric.Realtime.Data.Stores
{
    using Fabric.Realtime.Domain;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The realtime database context.
    /// </summary>
    /// <remarks>
    /// A DbContext instance represents a session with the database and can be used to 
    /// query and save instances of your entities. DbContext is a combination of 
    /// the Unit Of Work and Repository patterns.
    /// </remarks>
    public class RealtimeContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RealtimeContext"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public RealtimeContext(DbContextOptions<RealtimeContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the HL7 messages.
        /// </summary>
        public DbSet<HL7Message> HL7Messages { get; set; }

        /// <summary>
        /// Gets or sets the subscriptions.
        /// </summary>
        public DbSet<RealtimeSubscription> Subscriptions { get; set; }

        /// <summary>
        /// Gets or sets the forwarding history.
        /// </summary>
        public DbSet<ForwardingHistory> ForwardingHistory { get; set; }

        /// <summary>
        /// Override to configure the entity model.
        /// </summary>
        /// <param name="builder">
        /// The model builder instance used for constructing a model for a context.
        /// </param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var realtimeModelBuilder = new RealtimeModelBuilder();
            realtimeModelBuilder.BuildModel(builder);
        }
    }
}
