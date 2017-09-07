namespace Fabric.Realtime.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Fabric.Realtime.Domain.Models;
    using Fabric.Realtime.Domain.Stores;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/v1/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly RealtimeContext _realtimeContext;

        public SubscriptionController(RealtimeContext realtimeContext)
        {
            this._realtimeContext = realtimeContext;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Subscription> Get()
        {
            // Return simple list of messages for demo purposes
            var setOfSubscriptions = this._realtimeContext.Subscriptions.Include(o => o.MessageEvents);
            var simpleListOfMessages = setOfSubscriptions.ToList();
            return simpleListOfMessages.ToArray();
        }

        [HttpPost]
        public void Post([FromBody] Subscription subscription)
        {
            this._realtimeContext.Subscriptions.Add(subscription);
            subscription.SubscriptionDate = DateTime.UtcNow;
            this._realtimeContext.SaveChanges();
        }
    }
}