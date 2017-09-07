namespace Fabric.Realtime.Domain.Stores
{
    using Fabric.Realtime.Domain.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.Extensions.DependencyInjection;

    public class RealtimeContext : DbContext
    {
        public RealtimeContext(DbContextOptions<RealtimeContext> options) : base(options) { }

        public DbSet<HL7Message> HL7Messages { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<HL7Message>(ConfigureHL7Message);
            builder.Entity<SubscriptionMessageEvent>(this.ConfigureSubscriptionMessageEvent);
            builder.Entity<Subscription>(ConfigureSubscription);
        }

        void ConfigureSubscription(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable("Subscription");
            builder.Property(ci => ci.Id)
                .UseSqlServerIdentityColumn()
                .IsRequired();
            builder.Property(ci => ci.SubscriptionName).IsRequired().HasMaxLength(255);
            builder.Property(ci => ci.SubscriptionDate).IsRequired();
            builder.Property(ci => ci.RoutingKey).IsRequired().HasMaxLength(255);
            builder.HasMany(ci => ci.MessageEvents);
        }

        void ConfigureSubscriptionMessageEvent(EntityTypeBuilder<SubscriptionMessageEvent> builder)
        {
            builder.ToTable("SubscriptionMessageEvent");
            builder.Property(ci => ci.Id).UseSqlServerIdentityColumn().IsRequired();
            builder.Property(ci => ci.MessageEvent).IsRequired().HasMaxLength(255);
        }

        void ConfigureHL7Message(EntityTypeBuilder<HL7Message> builder)
        {
            builder.ToTable("HL7Message");

            builder.Property(ci => ci.Id)
                .UseSqlServerIdentityColumn()
                .IsRequired();

            builder.Property(ci => ci.MessageHash)
                .IsRequired(true)
                .HasMaxLength(255);

            builder.Property(ci => ci.ProtocolVersion)
                .IsRequired(true)
                .HasMaxLength(255);

            builder.Property(ci => ci.RawMessage)
                .IsRequired(true)
                .HasMaxLength(8000);

            builder.Property(ci => ci.TransmissionReceiptTime)
                .IsRequired(true);

            builder.Property(ci => ci.ExternalPatientID)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(ci => ci.InternalPatientID)
                .IsRequired(true)
                .HasMaxLength(255);

            builder.Property(ci => ci.MessageDate)
                .IsRequired(false);

            builder.Property(ci => ci.MessageEvent)
                .IsRequired(true)
                .HasMaxLength(255);

            builder.Property(ci => ci.MessageType)
                .IsRequired(true)
                .HasMaxLength(255);

            builder.Property(ci => ci.ReceivingApplication)
                .IsRequired(true)
                .HasMaxLength(255);

            builder.Property(ci => ci.XmlMessage)
                .IsRequired(true)
                .HasMaxLength(8000);

        }
    }
}
