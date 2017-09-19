namespace Fabric.Realtime.Engine.Configuration
{
    /// <summary>
    /// The interface engine settings.
    /// </summary>
    public class InterfaceEngineSettings
    {
        /// <summary>
        /// Gets or sets the exchange name.
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// Gets or sets the queue name.
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// Gets or sets the routing key.
        /// </summary>
        public string RoutingKey { get; set; }
    }
}