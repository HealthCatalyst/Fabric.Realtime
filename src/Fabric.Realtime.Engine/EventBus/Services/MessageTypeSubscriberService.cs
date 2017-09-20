namespace Fabric.Realtime.Engine.EventBus.Services
{
    using System.Collections.Generic;

    using Fabric.Realtime.Data.Stores;
    using Fabric.Realtime.Engine.EventBus.Models;

    using Microsoft.EntityFrameworkCore;

    using RabbitMQ.Client.MessagePatterns;
    using Fabric.Realtime.Domain;

    public class MessageTypeSubscriberService : IInitializable
    {
        private readonly RealtimeContext context;

        public MessageTypeSubscriberService(RealtimeContext context)
        {
            this.context = context;
            this.EventSubscriptionDictionary = new Dictionary<string, List<RealtimeSubscription>>();
        }

        public Dictionary<string, List<RealtimeSubscription>> EventSubscriptionDictionary { get; set; }

        public void AddSubscription(RealtimeSubscription subscription)
        {
            // TODO Implement AddSubscription
            ////if (subscription.MessageTypes == null) return;
            ////foreach (var messageType in subscription.MessageTypes)
            ////    if (!this.EventSubscriptionDictionary.ContainsKey(messageType.MessageType))
            ////    {
            ////        this.EventSubscriptionDictionary.Add(
            ////            messageType.MessageType,
            ////            new List<Subscription> { subscription });
            ////    }
            ////    else
            ////    {
            ////        if (this.EventSubscriptionDictionary.TryGetValue(
            ////            messageType.MessageType,
            ////            out List<Subscription> subscriptionList)) subscriptionList.Add(subscription);
            ////    }
        }

        public List<RealtimeSubscription> GetSubscriptions(string messageEvent)
        {
            if (this.EventSubscriptionDictionary.TryGetValue(messageEvent, out List<RealtimeSubscription> subscriptionList))
                return subscriptionList;
            return new List<RealtimeSubscription>();
        }

        public void Initialize()
        {
            var subscriptions = this.context.Subscriptions;
            foreach (var subscription in subscriptions)
            {
                this.AddSubscription(subscription);
            }
        }
    }
}