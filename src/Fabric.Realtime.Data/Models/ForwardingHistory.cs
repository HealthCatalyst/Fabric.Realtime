using System;
using System.Collections.Generic;
using System.Text;

namespace Fabric.Realtime.Data.Models
{
    public class ForwardingHistory
    {
        public long Id { get; set; }

        public long MessageId { get; set; }

        public int SubscriptionId { get; set; }

        public DateTimeOffset Sent { get; set; }
    }

}
