namespace Fabric.Realtime.Engine.Record
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Fabric.Realtime.Core;
    using Fabric.Realtime.Core.Utils;
    using Fabric.Realtime.Engine.Configuration;
    using Fabric.Realtime.Engine.Handlers;

    using Microsoft.Extensions.Logging;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using RabbitMQ.Client.MessagePatterns;

    /// <summary>
    /// Listens for incoming messages and saves them to a persistent store.
    /// </summary>
    public class MessageReceiveWorker : IBackgroundWorker
    {
        /// <summary>
        /// The timeout in milliseconds to wait for messages from the broker.
        /// </summary>
        private const int SubscriptionNextTimeoutMilliseconds = 100;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

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
        /// Initializes a new instance of the <see cref="MessageReceiveWorker"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="eventHandler">
        /// The event handler for callback for processing incoming messages.
        /// </param>
        public MessageReceiveWorker(
            ILogger<MessageReceiveWorker> logger,
            IRealtimeConfiguration configuration,
            IInterfaceEngineEventHandler eventHandler)
        {
            Guard.ArgumentNotNull(logger, nameof(logger));

            this.logger = logger;
            this.configuration = configuration;
            this.eventHandler = eventHandler;
            this.connectionFactory =
                new ConnectionFactory { Uri = new Uri(this.configuration.MessageBrokerSettings.AmqpUri) };
        }

        /// <summary>
        /// Gets the exchange name.
        /// </summary>
        public string ExchangeName => this.configuration.InterfaceEngineSettings.ExchangeName;

        // TODO Make exchange type configuration setting.

        /// <summary>
        /// Gets the exchange type.
        /// </summary>
        public string ExchangeType { get; } = "direct";

        /// <summary>
        /// The queue name.
        /// </summary>
        public string QueueName => this.configuration.InterfaceEngineSettings.QueueName;

        /// <summary>
        /// The routing key.
        /// </summary>
        public string RoutingKey => this.configuration.InterfaceEngineSettings.RoutingKey;

        /// <inheritdoc />
        public async Task RunAsync(CancellationToken token)
        {
            try
            {
                while (true)
                {
                    try
                    {
                        await this.ReceiveMessagesAsync(token);
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogWarning(ex, ex.Message);
                    }

                    // Retry connection every 5 seconds
                    this.logger.LogInformation("Waiting for connection to AMQP broker...");
                    await Task.Delay(5000, token);
                }
            }
            catch (TaskCanceledException)
            {
                this.logger.LogInformation("Exiting MessageReceiveWorker.");
            }
        }

        /// <summary>
        /// Listen for incoming messages.
        /// </summary>
        /// <param name="token">
        /// The cancellation token to signal the worker to exit.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task ReceiveMessagesAsync(CancellationToken token)
        {
            using (var connection = this.connectionFactory.CreateConnection())
            using (var amqpModel = connection.CreateModel())
            {
                amqpModel.ExchangeDeclare(this.ExchangeName, this.ExchangeType);
                amqpModel.QueueDeclare(this.QueueName, true, false, false, null);
                amqpModel.BasicQos(0, 1, false);

                amqpModel.QueueBind(this.QueueName, this.ExchangeName, this.RoutingKey);

                this.logger.LogInformation($"Waiting for messages on queue '{this.QueueName}' with routing key '{this.RoutingKey}'.");

                var amqpSubscription = new Subscription(amqpModel, this.QueueName);
                while (true)
                {
                    var response = amqpSubscription.Next(SubscriptionNextTimeoutMilliseconds, out BasicDeliverEventArgs ea);
                    if (response == false)
                    {
                        if (amqpModel.IsClosed)
                        {
                            this.logger.LogWarning($"AMQP channel {amqpModel.ChannelNumber} is closed.");
                            return;
                        }

                        continue;
                    }

                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    this.logger.LogDebug($"RECEIVE: {ea.RoutingKey}: {message}");

                    var success = this.eventHandler.HandleMessage(message);
                    if (success)
                    {
                        amqpSubscription.Ack(ea);
                    }

                    ////await task;
                    ////var success = task.IsCompleted;
                    ////if (success)
                    ////{
                    ////    amqpModel.BasicAck(ea.DeliveryTag, false);
                    ////}

                    await Task.Delay(10, token);
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
