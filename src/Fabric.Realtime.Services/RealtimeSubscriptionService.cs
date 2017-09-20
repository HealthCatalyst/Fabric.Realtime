namespace Fabric.Realtime.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Fabric.Realtime.Core.Utils;
    using Fabric.Realtime.Data.Stores;
    using Fabric.Realtime.Domain;

    using Microsoft.EntityFrameworkCore;

    /// <inheritdoc />
    public class RealtimeSubscriptionService : IRealtimeSubscriptionService
    {
        /// <summary>
        /// The database context.
        /// </summary>
        private readonly RealtimeContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RealtimeSubscriptionService"/> class.
        /// </summary>
        /// <param name="context">
        /// The database context.
        /// </param>
        public RealtimeSubscriptionService(RealtimeContext context)
        {
            Guard.ArgumentNotNull(context, nameof(context));
            this.context = context;
        }

        /// <inheritdoc />
        public void Add(RealtimeSubscription subscription)
        {
            Guard.ArgumentNotNull(subscription, nameof(subscription));

            // Update fields
            var timestamp = DateTimeOffset.Now;
            subscription.IsActive = true;
            subscription.CreatedOn = timestamp;
            subscription.LastModifiedOn = timestamp;

            this.context.Subscriptions.Add(subscription);
            this.context.SaveChanges();
        }

        /// <inheritdoc />
        public RealtimeSubscription FindById(long id)
        {
            return this.context.Subscriptions.AsNoTracking().FirstOrDefault(subscription => subscription.Id == id);
        }

        /// <inheritdoc />
        public IEnumerable<RealtimeSubscription> FindBySourceMessageType(string sourceType)
        {
            return this.context.Subscriptions.AsNoTracking()
                .Where(subscription => subscription.SourceMessageType == sourceType)
                .ToList();
        }
    }
}