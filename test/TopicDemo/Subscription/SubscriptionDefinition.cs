namespace Subscribe
{
    /// <summary>
    /// The subscription definition.
    /// </summary>
    public class SubscriptionDefinition
    {
        /// <summary>
        /// Gets or sets the message broker configuration.
        /// </summary>
        public BrokerHostConfiguration Broker { get; set; }

        /// <summary>
        /// Gets or sets the exchange definition.
        /// </summary>
        public ExchangeDefinition Exchange { get; set; }

        /// <summary>
        /// Gets or sets the queue definition.
        /// </summary>
        public QueueDefinition Queue { get; set; }

        /// <summary>
        /// Gets or sets the routing key.
        /// </summary>
        public string RoutingKey { get; set; }
    }
}