using System;

namespace Fabric.Realtime.Data.Models
{
    public class Subscription
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string SourceMessageType { get; set; }

        public string MessageFormat { get; set; }

        public string RoutingKey { get; set; }

        public DateTimeOffset LastModified { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTimeOffset Created { get; set; }

        public string CreatedBy { get; set; }

        ////[Required]
        ////public int MessageExpiration { get; set; }

        ////public IEnumerable<ForwardingHistory> ForwardingHistory { get; set; }
    }

}
