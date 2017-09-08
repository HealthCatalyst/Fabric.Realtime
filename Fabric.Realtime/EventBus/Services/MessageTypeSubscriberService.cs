namespace Fabric.Realtime.EventBus.Services
{
    using System.Collections.Generic;

    using Fabric.Realtime.Domain.Models;
    using Fabric.Realtime.Domain.Stores;
    using Fabric.Realtime.EventBus.Models;

    using Microsoft.EntityFrameworkCore;

    public class MessageTypeSubscriberService : IInitializable
    {
        private readonly RealtimeContext _context;

        public MessageTypeSubscriberService(RealtimeContext context)
        {
            this._context = context;
            this.EventSubscriptionDictionary = new Dictionary<string, List<Subscription>>();
        }

        public Dictionary<string, List<Subscription>> EventSubscriptionDictionary { get; set; }

        public void AddSubscription(Subscription subscription)
        {
            if (subscription.MessageTypes == null) return;
            foreach (var messageType in subscription.MessageTypes)
                if (!this.EventSubscriptionDictionary.ContainsKey(messageType.MessageType))
                {
                    this.EventSubscriptionDictionary.Add(
                        messageType.MessageType,
                        new List<Subscription> { subscription });
                }
                else
                {
                    if (this.EventSubscriptionDictionary.TryGetValue(
                        messageType.MessageType,
                        out List<Subscription> subscriptionList)) subscriptionList.Add(subscription);
                }
        }

        public List<Subscription> GetSubscriptions(string messageEvent)
        {
            if (this.EventSubscriptionDictionary.TryGetValue(messageEvent, out List<Subscription> subscriptionList))
                return subscriptionList;
            return new List<Subscription>();
        }

        public void Initialize()
        {
            var subscriptions = this._context.Subscriptions.Include(o => o.MessageTypes);
            foreach (var subscription in subscriptions) this.AddSubscription(subscription);
        }
    }
}