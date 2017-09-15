using Fabric.Realtime.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fabric.Realtime.Data.Stores
{
    public class RealtimeModelBuilder
    {
        public void BuildModel(ModelBuilder builder)
        {
            builder.Entity<HL7Message>(ConfigureHL7Message);
            builder.Entity<Subscription>(ConfigureSubscription);
            builder.Entity<ForwardingHistory>(ConfigureForwardingHistory);
        }

        private static void ConfigureHL7Message(EntityTypeBuilder<HL7Message> builder)
        {
            builder.ToTable(nameof(HL7Message));

            builder.Property(ci => ci.Id)
                .UseSqlServerIdentityColumn()
                .IsRequired();

            builder.Property(ci => ci.MessageHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ci => ci.MessageVersion)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ci => ci.MessageType)
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

            builder.Property(ci => ci.Sent)
                .IsRequired();
        }

        private static void ConfigureSubscription(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable(nameof(Subscription));

            builder.Property(ci => ci.Id)
                .UseSqlServerIdentityColumn()
                .IsRequired();

            builder.Property(ci => ci.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ci => ci.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(ci => ci.SourceMessageType)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ci => ci.MessageFormat)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ci => ci.RoutingKey)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ci => ci.LastModified)
                .IsRequired();

            builder.Property(ci => ci.LastModifiedBy)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ci => ci.Created)
                .IsRequired();

            builder.Property(ci => ci.CreatedBy)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasMany(typeof(ForwardingHistory)).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}