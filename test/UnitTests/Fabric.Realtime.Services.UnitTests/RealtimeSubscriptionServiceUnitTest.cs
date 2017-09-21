namespace Fabric.Realtime.Services.UnitTests
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Fabric.Realtime.Data.Stores;
    using Fabric.Realtime.Domain;
    using Fabric.Realtime.Services;

    using Microsoft.EntityFrameworkCore;

    using Xunit;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class RealtimeSubscriptionServiceUnitTest
    {
        [Fact]
        public void RealtimeSubscriptionServiceFindById()
        {
            using (var ctx = GetContextWithData())
            {
                var service = new RealtimeSubscriptionService(ctx);
                var subscription = service.FindById(1);

                Assert.NotNull(subscription);
                Assert.Equal(1, subscription.Id);
                Assert.Equal("HL7", subscription.SourceMessageType);
                Assert.Equal("RAW", subscription.MessageFormat);
                Assert.True(subscription.IsActive);
            }
        }

        [Fact]
        public void RealtimeSubscriptionServiceAddSubscription()
        {
            var expected = new RealtimeSubscription
            {
                Name = "A",
                SourceMessageType = "HL7",
                MessageFormat = "RAW",
                RoutingKey = "HL7.{MessageType}.{EventType}"
            };

            using (var ctx = CreateRealtimeContext())
            {
                var service = new RealtimeSubscriptionService(ctx);
                service.Add(expected);

                var actual = service.FindById(expected.Id);

                Assert.NotNull(actual);
                Assert.True(actual.Id > 0);
                Assert.Equal("HL7", actual.SourceMessageType);
                Assert.Equal("RAW", actual.MessageFormat);
                Assert.True(actual.IsActive);
            }
        }

        [Fact]
        public void RealtimeSubscriptionServiceAddNullSubscription()
        {
            using (var ctx = CreateRealtimeContext())
            {
                var service = new RealtimeSubscriptionService(ctx);
                var exception = Record.Exception(() => service.Add(null));
                Assert.NotNull(exception);
                Assert.IsType<ArgumentNullException>(exception);
                Assert.Equal("subscription", ((ArgumentNullException)exception).ParamName);
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
            AddSubscriptions(ctx);
            ctx.SaveChanges();
            return ctx;
        }

        private static RealtimeContext CreateRealtimeContext()
        {
            return new RealtimeContext(
                new DbContextOptionsBuilder<RealtimeContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        }

        private static void AddSubscriptions(RealtimeContext ctx)
        {
            ctx.Subscriptions.Add(
                new RealtimeSubscription
                {
                    Id = 1,
                    Name = "A",
                    IsActive = true,
                    SourceMessageType = "HL7",
                    MessageFormat = "RAW",
                    RoutingKey = "HL7.{MessageType}.{EventType}"
                });
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
