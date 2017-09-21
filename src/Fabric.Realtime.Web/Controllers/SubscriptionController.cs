namespace Fabric.Realtime.Web.Controllers
{
    // ReSharper disable StyleCop.SA1650
    using System;
    using System.Collections.Generic;

    using Fabric.Realtime.Domain;
    using Fabric.Realtime.Services;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The subscription controller.
    /// </summary>
    [Route("api/v1/[controller]")]
    public class SubscriptionController : Controller
    {
        /// <summary>
        /// The subscription management service.
        /// </summary>
        private readonly IRealtimeSubscriptionService subscriptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionController"/> class.
        /// </summary>
        /// <param name="subscriptionService">
        /// The subscription service.
        /// </param>
        public SubscriptionController(IRealtimeSubscriptionService subscriptionService)
        {
            this.subscriptionService = subscriptionService;
        }

        /// <summary>
        /// GET api/v1/subscription.
        /// </summary>
        /// <returns>
        /// An enumerable of <see cref="RealtimeSubscription"/> entities.
        /// </returns>
        [HttpGet]
        public IEnumerable<RealtimeSubscription> Get()
        {
            // Return simple list of messages for demo purposes
            return this.subscriptionService.GetAll();
        }

        /// <summary>
        /// HTTP POST api/v1/subscription.
        /// </summary>
        /// <param name="subscription">
        /// The subscription.
        /// </param>
        [HttpPost]
        public void Post([FromBody] RealtimeSubscription subscription)
        {
            // Set timestamps and save to database
            subscription.CreatedOn = DateTimeOffset.Now;
            subscription.LastModifiedOn = subscription.CreatedOn;
            this.subscriptionService.Add(subscription);
        }
    }
}