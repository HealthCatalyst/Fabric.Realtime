namespace Subscribe
{
    /// <summary>
    /// The message event.
    /// </summary>
    public class MessageReceivedEvent
    {
        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        public byte[] Body { get; set; }

        /// <summary>
        /// Gets or sets the consumer tag.
        /// </summary>
        public string ConsumerTag { get; set; }

        /// <summary>
        /// Gets or sets the delivery tag.
        /// </summary>
        public ulong DeliveryTag { get; set; }

        /// <summary>
        /// Gets or sets the exchange.
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether redelivered.
        /// </summary>
        public bool Redelivered { get; set; }

        /// <summary>
        /// Gets or sets the routing key used when the message was 
        /// originally published.
        /// </summary>
        public string RoutingKey { get; set; }
    }
}