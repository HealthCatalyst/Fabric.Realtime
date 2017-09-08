namespace Fabric.Realtime.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Fabric.Realtime.Domain.Models;
    using Fabric.Realtime.Domain.Stores;
    using Fabric.Realtime.EventBus.Services;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/v1/[controller]")]
    public class SubscriptionController : Controller
    {
        private readonly MessageTypeSubscriberService _messageTypeSubscriberService;

        private readonly RealtimeContext _realtimeContext;

        public SubscriptionController(
            RealtimeContext realtimeContext,
            MessageTypeSubscriberService messageTypeSubscriberService)
        {
            this._realtimeContext = realtimeContext;
            this._messageTypeSubscriberService = messageTypeSubscriberService;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Subscription> Get()
        {
            // Return simple list of messages for demo purposes
            var setOfSubscriptions = this._realtimeContext.Subscriptions.Include(o => o.MessageTypes);
            var simpleListOfMessages = setOfSubscriptions.ToList();
            return simpleListOfMessages.ToArray();
        }

        [HttpPost]
        public void Post([FromBody] Subscription subscription)
        {
            // Save to database
            this._realtimeContext.Subscriptions.Add(subscription);
            subscription.SubscriptionDate = DateTime.UtcNow;
            this._realtimeContext.SaveChanges();

            // Add to active message type subscriber service
            this._messageTypeSubscriberService.AddSubscription(subscription);
        }
    }
}