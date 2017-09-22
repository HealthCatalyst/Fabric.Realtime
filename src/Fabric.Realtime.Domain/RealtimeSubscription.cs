namespace Fabric.Realtime.Domain
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A message forwarding subscription.
    /// </summary>
    public class RealtimeSubscription
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the subscription name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// subscription is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the source message type (e.g. 'HL7', 'X12', etc.)
        /// </summary>
        public string SourceMessageType { get; set; }

        /// <summary>
        /// Gets or sets the message format (e.g. 'RAW' or 'XML').
        /// </summary>
        public string MessageFormat { get; set; }

        /// <summary>
        /// Gets or sets the routing key.
        /// </summary>
        public string RoutingKey { get; set; }

        /// <summary>
        /// Gets or sets the last modified timestamp.
        /// </summary>
        public DateTimeOffset LastModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the last modified user.
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the created timestamp.
        /// </summary>
        public DateTimeOffset CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the creation user.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the forwarding history.
        /// </summary>
        public ICollection<ForwardingHistory> ForwardingHistory { get; set; }
    }

}
