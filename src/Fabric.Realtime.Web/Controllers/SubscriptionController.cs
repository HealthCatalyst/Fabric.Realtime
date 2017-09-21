namespace Fabric.Realtime.Web.Controllers
{
    // ReSharper disable StyleCop.SA1650
    using System;
    using System.Collections.Generic;

    using Fabric.Realtime.Domain;
    using Fabric.Realtime.Services;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.KeyVault.Models;

    using Swashbuckle.AspNetCore.SwaggerGen;

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
        /// Gets all subscriptions.
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
        /// Gets a subscription by id.
        /// </summary>
        /// <param name="id">
        /// The unique identifier.
        /// </param>
        /// <returns>
        /// The <see cref="RealtimeSubscription"/>.
        /// </returns>
        [HttpGet("{id}")]
        public RealtimeSubscription Get(long id)
        {
            // Return simple list of messages for demo purposes
            return this.subscriptionService.FindById(id);
        }

        /// <summary>
        /// Add a new subscription.
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

        /// <summary>
        /// Update an existing subscription.
        /// </summary>
        /// <param name="subscription">
        /// The subscription.
        /// </param>
        [HttpPut]
        public void Put([FromBody] RealtimeSubscription subscription)
        {
        }

        /// <summary>
        /// Delete an existing subscription.
        /// </summary>
        /// <param name="id">
        /// The subscription unique identifier.
        /// </param>
        [HttpDelete]
        public void Delete(long id)
        {
        }
    }
}