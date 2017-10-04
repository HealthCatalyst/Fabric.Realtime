namespace Subscribe
{
    using RabbitMQ.Client;
    using RabbitMQ.Client.MessagePatterns;

    /// <summary>
    /// An AMQP message broker factory interface.
    /// </summary>
    public interface IBrokerFactory
    {
        /// <summary>
        /// Create a new connection.
        /// </summary>
        /// <param name="configuration">
        /// The message broker connection configuration.
        /// </param>
        /// <returns>
        /// A new <see cref="IConnection"/> instance.
        /// </returns>
        IConnection CreateConnection(BrokerHostConfiguration configuration);

        /// <summary>
        /// Creates a new subscription.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="queueName">
        /// The queue name.
        /// </param>
        /// <param name="autoAck">
        /// If true, automatically acknowledge messages.
        /// </param>
        /// <returns>
        /// A new <see cref="ISubscription"/> instance.
        /// </returns>
        ISubscription CreateSubscription(IModel model, string queueName, bool autoAck);
    }
}