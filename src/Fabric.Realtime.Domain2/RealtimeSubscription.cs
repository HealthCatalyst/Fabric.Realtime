namespace Fabric.Realtime.Domain
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A message forwarding subscription.
    /// </summary>
    public class RealtimeSubscription
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string SourceMessageType { get; set; }

        public string MessageFormat { get; set; }

        public string RoutingKey { get; set; }

        public DateTimeOffset LastModifiedOn { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public ICollection<ForwardingHistory> ForwardingHistory { get; set; }
    }

}
