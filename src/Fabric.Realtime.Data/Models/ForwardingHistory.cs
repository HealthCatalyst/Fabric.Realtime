namespace Fabric.Realtime.Data.Models
{
    using System;

    public class ForwardingHistory
    {
        public long Id { get; set; }

        public long MessageId { get; set; }

        public int SubscriptionId { get; set; }

        public DateTimeOffset SentOn { get; set; }

        public Subscription Subscription { get; set; }
    }

}
