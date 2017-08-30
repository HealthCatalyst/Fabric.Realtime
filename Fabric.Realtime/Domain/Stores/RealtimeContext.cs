namespace Fabric.Realtime.Domain.Stores
{
    using Fabric.Realtime.Domain.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class RealtimeContext : DbContext
    {
        public RealtimeContext(DbContextOptions<RealtimeContext> options) : base(options) { }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Message>(ConfigureMessage);
        }

        void ConfigureMessage(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Message");

            builder.Property(ci => ci.Id)
                .UseSqlServerIdentityColumn()
                .IsRequired();

            builder.Property(ci => ci.MessageHash)
                .IsRequired(true)
                .HasMaxLength(255);

            builder.Property(ci => ci.Protocol)
                .IsRequired(true)
                .HasMaxLength(255);

            builder.Property(ci => ci.RawMessage)
                .IsRequired(true)
                .HasMaxLength(8000);

            builder.Property(ci => ci.TransmissionReceiptTime)
                .IsRequired(true);

            builder.Property(ci => ci.Version)
                .IsRequired(true)
                .HasMaxLength(255);

            builder.Property(ci => ci.XmlMessage)
                .IsRequired(true)
                .HasMaxLength(8000);

        }
    }
}
