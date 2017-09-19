namespace Fabric.Realtime.Data.UnitTests
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Fabric.Realtime.Data.Models;
    using Fabric.Realtime.Data.Stores;

    using Microsoft.EntityFrameworkCore;

    using Xunit;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            using (var ctx = this.GetContextWithData())
            {
                var msg = ctx.HL7Messages.Find((long)1);
                Assert.NotNull(msg);
            }
        }

        /// <summary>
        /// Creates an in-memory database context for unit testing.
        /// </summary>
        /// <returns>
        /// The <see cref="RealtimeContext"/>.
        /// </returns>
        private RealtimeContext GetContextWithData()
        {
            var ctx = new RealtimeContext(
                new DbContextOptionsBuilder<RealtimeContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            ctx.HL7Messages.Add(
                new HL7Message
                {
                    Id = 1,
                    Protocol = "HL7",
                    ProtocolVersion = "2.50",
                    MessageType = "ADT",
                    MessageHash = Guid.NewGuid().ToString("N"),
                    TransmissionReceiptTime = DateTimeOffset.Now
                });
            ctx.HL7Messages.Add(
                new HL7Message
                {
                    Id = 2,
                    Protocol = "HL7",
                    ProtocolVersion = "2.50",
                    MessageType = "ADT",
                    MessageHash = Guid.NewGuid().ToString("N"),
                    TransmissionReceiptTime = DateTimeOffset.Now
                });
            return ctx;
        }
    }
}
