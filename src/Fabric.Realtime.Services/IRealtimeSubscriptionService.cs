namespace Fabric.Realtime.Services
{
    using System.Collections.Generic;

    using Fabric.Realtime.Domain;

    /// <summary>
    /// The RealtimeSubscriptionService interface.
    /// </summary>
    public interface IRealtimeSubscriptionService
    {
        /// <summary>
        /// Add a new subscription to the data store.
        /// </summary>
        /// <param name="subscription">
        /// The subscription.
        /// </param>
        void Add(RealtimeSubscription subscription);

        /// <summary>
        /// Gets all subscriptions.
        /// </summary>
        /// <returns>
        /// An enumerable of <see cref="RealtimeSubscription"/> entities.
        /// </returns>
        IEnumerable<RealtimeSubscription> GetAll();

        /// <summary>
        /// Finds a subscription by the given identifier.
        /// </summary>
        /// <param name="id">
        /// The unique subscription identifier.
        /// </param>
        /// <returns>
        /// The <see cref="RealtimeSubscription"/>, or null if not found.
        /// </returns>
        RealtimeSubscription FindById(long id);

        /// <summary>
        /// The find by source message type.
        /// </summary>
        /// <param name="sourceType">
        /// The source message type (e.g. "HL7", "X12", etc.).
        /// </param>
        /// <returns>
        /// An enumerable of <see cref="RealtimeSubscription"/> entities.
        /// </returns>
        IEnumerable<RealtimeSubscription> FindBySourceMessageType(string sourceType);
    }
}
