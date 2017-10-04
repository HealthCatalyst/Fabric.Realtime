namespace Subscribe
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The Subscriber interface.
    /// </summary>
    public interface ISubscriber
    {
        /// <summary>
        /// Async task for receiving messages from a message exchange.
        /// </summary>
        /// <param name="subscription">
        /// The subscription definition.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <param name="handler">
        /// The function invoked whenever a new message arrives.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/> for this async operation.
        /// </returns>
        Task RunAsync(SubscriptionDefinition subscription, CancellationToken cancellationToken, Func<SubscriberMessage, bool> handler);
    }
}