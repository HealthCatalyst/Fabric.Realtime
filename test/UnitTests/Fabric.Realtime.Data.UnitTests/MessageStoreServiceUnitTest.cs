namespace Fabric.Realtime.Data.UnitTests
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Fabric.Realtime.Data.Stores;
    using Fabric.Realtime.Domain;
    using Fabric.Realtime.Services;

    using Microsoft.EntityFrameworkCore;

    using Xunit;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class MessageStoreServiceUnitTest
    {
        [Fact]
        public void MessageStoreServiceFindById()
        {
            using (var ctx = GetContextWithData())
            {
                var service = new MessageStoreService(ctx);
                var msg = service.FindById(3);

                Assert.NotNull(msg);
                Assert.Equal(3, msg.Id);
                Assert.Equal("HL7", msg.Protocol);
                Assert.Equal("2.8", msg.ProtocolVersion);
                Assert.Equal("ADT", msg.MessageType);
                Assert.True(!string.IsNullOrEmpty(msg.MessageHash));
            }
        }

        [Fact]
        public void MessageStoreServiceInsert()
        {
            var expectedMessage = new HL7Message
            {
                Protocol = "HL7",
                ProtocolVersion = "2.6",
                MessageType = "ADT",
                MessageHash = Guid.NewGuid().ToString("N"),
                TransmissionReceiptTime = new DateTimeOffset(new DateTime(2017, 1, 1, 0, 0, 0).ToUniversalTime())
            };

            using (var ctx = CreateRealtimeContext())
            {
                var service = new MessageStoreService(ctx);
                service.Insert(expectedMessage);
            }
        }

        [Fact]
        public void MessageStoreServiceInsertNull()
        {
            using (var ctx = CreateRealtimeContext())
            {
                var service = new MessageStoreService(ctx);
                var exception = Record.Exception(() => service.Insert(null));
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
                Assert.Equal("message", ((ArgumentNullException)exception).ParamName);
            }
        }

        /// <summary>
        /// Creates an in-memory database context for unit testing.
        /// </summary>
        /// <returns>
        /// The <see cref="RealtimeContext"/>.
        /// </returns>
        private static RealtimeContext GetContextWithData()
        {
            var ctx = CreateRealtimeContext();
            AddMessages(ctx);
            ctx.SaveChanges();
            return ctx;
        }

        private static RealtimeContext CreateRealtimeContext()
        {
            return new RealtimeContext(
                new DbContextOptionsBuilder<RealtimeContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        }

        private static void AddMessages(RealtimeContext ctx)
        {
            var tm = new DateTimeOffset(new DateTime(2017, 1, 1, 0, 0, 0).ToUniversalTime());
            ctx.HL7Messages.Add(
                new HL7Message
                {
                    Id = 1,
                    Protocol = "HL7",
                    ProtocolVersion = "2.5",
                    MessageType = "ADT",
                    MessageHash = Guid.NewGuid().ToString("N"),
                    TransmissionReceiptTime = tm
                });
            ctx.HL7Messages.Add(
                new HL7Message
                {
                    Id = 2,
                    Protocol = "HL7",
                    ProtocolVersion = "2.5",
                    MessageType = "ADT",
                    MessageHash = Guid.NewGuid().ToString("N"),
                    TransmissionReceiptTime = tm.AddHours(1)
                });
            ctx.HL7Messages.Add(
                new HL7Message
                {
                    Id = 3,
                    Protocol = "HL7",
                    ProtocolVersion = "2.8",
                    MessageType = "ADT",
                    MessageHash = Guid.NewGuid().ToString("N"),
                    TransmissionReceiptTime = tm.AddHours(1)
                });
            ctx.HL7Messages.Add(
                new HL7Message
                {
                    Id = 4,
                    Protocol = "HL7",
                    ProtocolVersion = "2.6",
                    MessageType = "ADT",
                    MessageHash = Guid.NewGuid().ToString("N"),
                    TransmissionReceiptTime = tm.AddHours(1)
                });
        }
    }
}
