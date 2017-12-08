namespace Subscribe
{
    using RabbitMQ.Client;
    using RabbitMQ.Client.MessagePatterns;

    /// <summary>
    /// The broker factory implementation for live connections to RabbitMQ.
    /// </summary>
    public class BrokerFactory : IBrokerFactory
    {
        /// <inheritdoc />
        public IConnection CreateConnection(BrokerHostConfiguration configuration)
        {
            var factory = new ConnectionFactory
                              {
                                  HostName = configuration.Host,
                                  Port = configuration.Port,
                                  UserName = configuration.UserName,
                                  Password = configuration.Password,
                                  VirtualHost = configuration.VirtualHost
                              };
            return factory.CreateConnection();
        }

        /// <inheritdoc />
        public ISubscription CreateSubscription(IModel model, string queueName, bool autoAck)
        {
            return new Subscription(model, queueName, autoAck);
        }
    }
}
