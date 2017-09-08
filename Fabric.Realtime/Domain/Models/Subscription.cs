namespace Fabric.Realtime.Domain.Models
{
    using System;
    using System.Collections.Generic;

    public class Subscription
    {
        public int Id { get; set; }

        public List<SubscriptionMessageType> MessageTypes { get; set; }

        public string RoutingKey { get; set; }

        public DateTime SubscriptionDate { get; set; }

        public string SubscriptionName { get; set; }
    }
}