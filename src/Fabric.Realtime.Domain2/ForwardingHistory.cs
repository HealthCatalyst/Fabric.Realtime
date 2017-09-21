namespace Fabric.Realtime.Domain
{
    using System;

    /// <summary>
    /// The message forwarding history for a given subscription.
    /// </summary>
    public class ForwardingHistory
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the message id.
        /// </summary>
        public long MessageId { get; set; }

        /// <summary>
        /// Gets or sets the subscription id.
        /// </summary>
        public int SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the date and time the message was sent.
        /// </summary>
        public DateTimeOffset SentOn { get; set; }

        /// <summary>
        /// Gets or sets the associated subscription.
        /// </summary>
        public RealtimeSubscription RealtimeSubscription { get; set; }
    }

}
