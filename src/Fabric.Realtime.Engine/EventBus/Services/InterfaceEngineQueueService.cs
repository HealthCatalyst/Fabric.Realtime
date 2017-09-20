namespace Fabric.Realtime.Engine.EventBus.Services
{
    using System;
    using System.Text;

    using Fabric.Realtime.Engine.Configuration;
    using Fabric.Realtime.Engine.EventBus.Models;
    using Fabric.Realtime.Engine.Handlers;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    // TODO Make this a background service

    /// <summary>
    /// Receives messages from the incoming queue.
    /// </summary>
    public class InterfaceEngineQueueService : IDisposable, IInitializable
    {
        /// <summary>
        /// The event handler.
        /// </summary>
        private readonly IInterfaceEngineEventHandler eventHandler;

        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IRealtimeConfiguration configuration;

        /// <summary>
        /// The connection factory.
        /// </summary>
        private readonly ConnectionFactory connectionFactory;

        /// <summary>
        /// The AMQP model.
        /// </summary>
        private IModel channel;

        /// <summary>
        /// The AMQP connection.
        /// </summary>
        private IConnection connection;

        /// <summary>
        /// The consumer used to receive messages from a queue.
        /// </summary>
        private EventingBasicConsumer consumer;

        ////private readonly MessageBrokerExchangeClient interfaceEngine;

        public InterfaceEngineQueueService(
            IRealtimeConfiguration configuration,
            IInterfaceEngineEventHandler eventHandler)
        {
            this.configuration = configuration;
            this.connectionFactory =
                new ConnectionFactory { Uri = new Uri(this.configuration.MessageBrokerSettings.AmqpUri) };
            this.eventHandler = eventHandler;
        }

        /// <summary>
        /// Gets the exchange name.
        /// </summary>
        public string ExchangeName => this.configuration.InterfaceEngineSettings.ExchangeName;

        /// <summary>
        /// The queue name.
        /// </summary>
        public string QueueName => this.configuration.InterfaceEngineSettings.QueueName;

        /// <summary>
        /// The routing key.
        /// </summary>
        public string RoutingKey => this.configuration.InterfaceEngineSettings.RoutingKey;

        public void Dispose()
        {
            this.channel.Dispose();
            this.connection.Dispose();
        }

        public void Initialize()
        {
            this.connection = this.connectionFactory.CreateConnection();
            this.channel = this.connection.CreateModel();

            // TODO Make exchange type configuration setting.
            this.channel.ExchangeDeclare(this.ExchangeName, "direct");
            this.channel.QueueDeclare(this.QueueName, true, false, false, null);
            this.channel.BasicQos(0, 1, false);

            this.channel.QueueBind(this.QueueName, this.ExchangeName, this.RoutingKey);

            this.consumer = new EventingBasicConsumer(this.channel);
            this.consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var task = this.eventHandler.HandleMessage(message);
                    var success = task.IsCompleted;
                    if (success)
                    {
                        this.channel.BasicAck(ea.DeliveryTag, false);
                    }
                };

            this.channel.BasicConsume(this.QueueName, false, this.consumer);
        }
     }
}