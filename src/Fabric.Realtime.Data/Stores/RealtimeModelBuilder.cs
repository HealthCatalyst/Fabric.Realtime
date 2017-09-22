namespace Fabric.Realtime.Data.Stores
{
    using Fabric.Realtime.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// The realtime model builder.
    /// </summary>
    public class RealtimeModelBuilder
    {
        /// <summary>
        /// Builds the Entity Framework model that defines the entities, 
        /// the relationships between them, and how they map to the database.
        /// </summary>
        /// <param name="builder">
        /// The model builder instance used for constructing a model for a context.
        /// </param>
        public void BuildModel(ModelBuilder builder)
        {
            builder.Entity<HL7Message>(ConfigureHL7Message);
            builder.Entity<RealtimeSubscription>(ConfigureSubscription);
            builder.Entity<ForwardingHistory>(ConfigureForwardingHistory);
        }

        /// <summary>
        /// Builds the model for HL7 messages.
        /// </summary>
        /// <param name="builder">
        /// The model builder instance used for constructing a model for a context.
        /// </param>
        private static void ConfigureHL7Message(EntityTypeBuilder<HL7Message> builder)
        {
            builder.ToTable(nameof(HL7Message));

            builder.Property(ci => ci.Id)
                .UseSqlServerIdentityColumn()
                .IsRequired();

            builder.Property(ci => ci.MessageHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ci => ci.ProtocolVersion)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ci => ci.Protocol)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ci => ci.EventType)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ci => ci.MessageDate)
                .IsRequired(false);

            builder.Property(ci => ci.TransmissionReceiptTime)
                .IsRequired();

            builder.Property(ci => ci.SendingApplication)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(ci => ci.SendingFacility)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(ci => ci.ReceivingApplication)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(ci => ci.ReceivingFacility)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(ci => ci.ExternalPatientID)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(ci => ci.InternalPatientID)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(ci => ci.MessageControlID)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(ci => ci.ProcessingID)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(ci => ci.RawMessage)
                .IsRequired();

            builder.Property(ci => ci.XmlMessage)
                .IsRequired(false);
        }

        /// <summary>
        /// Builds the model for message forwarding history.
        /// </summary>
        /// <param name="builder">
        /// The model builder instance used for constructing a model for a context.
        /// </param>
        private static void ConfigureForwardingHistory(EntityTypeBuilder<ForwardingHistory> builder)
        {
            builder.ToTable(nameof(ForwardingHistory));

            builder.Property(ci => ci.Id)
                .UseSqlServerIdentityColumn()
                .IsRequired();

            builder.Property(ci => ci.MessageId)
                .IsRequired();

            builder.Property(ci => ci.SubscriptionId)
                .IsRequired();

            builder.Property(ci => ci.SentOn)
                .IsRequired();
        }

        /// <summary>
        /// Builds the model for subscriptions.
        /// </summary>
        /// <param name="builder">
        /// The model builder instance used for constructing a model for a context.
        /// </param>
        private static void ConfigureSubscription(EntityTypeBuilder<RealtimeSubscription> builder)
        {
            builder.ToTable(nameof(RealtimeSubscription));

            builder.Property(ci => ci.Id)
                .UseSqlServerIdentityColumn()
                .IsRequired();

            builder.Property(ci => ci.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ci => ci.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(ci => ci.MessageFormat)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ci => ci.RoutingKey)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ci => ci.LastModifiedOn)
                .IsRequired();

            builder.Property(ci => ci.LastModifiedBy)
                .HasMaxLength(255);

            builder.Property(ci => ci.CreatedOn)
                .IsRequired();

            builder.Property(ci => ci.CreatedBy)
                .HasMaxLength(255);

            builder.HasMany(s => s.ForwardingHistory).WithOne(h => h.Subscription).OnDelete(DeleteBehavior.Cascade);
        }
    }
}