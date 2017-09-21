namespace Fabric.Realtime.Engine.Handlers
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    using Fabric.Realtime.Domain;
    using Fabric.Realtime.Engine.Configuration;
    using Fabric.Realtime.Engine.EventBus.Models;
    using Fabric.Realtime.Engine.Transformers;
    using Fabric.Realtime.Services;

    using Newtonsoft.Json;

    using RabbitMQ.Client;

    /// <summary>
    /// Receives incoming messages from the interface engine, persists them to the database,
    /// and forwards along to subscribers.
    /// </summary>
    public class InterfaceEngineEventHandler : IInterfaceEngineEventHandler
    {
        /// <summary>
        /// The configuration.
        /// </summary>
        private readonly IRealtimeConfiguration configuration;

        /// <summary>
        /// The RabbitMQ connection factory.
        /// </summary>
        private readonly ConnectionFactory connectionFactory;

        /// <summary>
        /// The realtime subscription persistence service.
        /// </summary>
        private readonly IRealtimeSubscriptionService subscriptionService;

        /// <summary>
        /// The message persistence service.
        /// </summary>
        private readonly IMessageStoreService messageStoreService;

        /// <summary>
        /// The message transformer.
        /// </summary>
        private readonly IInterfaceEngineMessageTransformer transformer;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceEngineEventHandler"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <param name="transformer">
        /// The transformer.
        /// </param>
        /// <param name="messageStoreService">
        /// The message store service.
        /// </param>
        /// <param name="subscriptionService">
        /// The subscription service.
        /// </param>
        public InterfaceEngineEventHandler(
            IRealtimeConfiguration configuration,
            IInterfaceEngineMessageTransformer transformer,
            IMessageStoreService messageStoreService,
            IRealtimeSubscriptionService subscriptionService)
        {
            this.transformer = transformer;
            this.messageStoreService = messageStoreService;
            this.subscriptionService = subscriptionService;
            this.configuration = configuration;
            this.connectionFactory =
                new ConnectionFactory { Uri = new Uri(this.configuration.MessageBrokerSettings.AmqpUri) };
        }

        /// <summary>
        /// The handle message.
        /// </summary>
        /// <param name="rawMessage">
        /// The raw message.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task HandleMessage(string rawMessage)
        {
            // 1. Deserialize
            InterfaceEngineMessage interfaceEngineMessage =
                JsonConvert.DeserializeObject<HL7InterfaceEngineMessage>(rawMessage);

            // 2. Transform
            HL7Message message = (HL7Message)this.transformer.Transform(interfaceEngineMessage);

            // 3. Persist
            if (message.Protocol.Equals(MessageProtocol.HL7.ToString()))
            {
                this.messageStoreService.Add(message);
            }

            // 4. Forward to interested parties via message exchange
            this.Foward(message);

            // 5. Return task
            return Task.CompletedTask;
        }

        /// <summary>
        /// Builds the routing key.
        /// </summary>
        /// <param name="subscription">
        /// The subscription.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string BuildRoutingKey(RealtimeSubscription subscription, HL7Message message)
        {
            return subscription.RoutingKey.Replace("{MessageType}", message.MessageType.Trim())
                .Replace("{EventType}", message.EventType.Trim());
        }

        /// <summary>
        /// Forward message to interested parties.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        private void Foward(HL7Message message)
        {
            var subscriptions = this.subscriptionService.FindBySourceMessageType(MessageProtocol.HL7.ToString());
            foreach (var subscription in subscriptions)
            {
                this.ForwardMessage(subscription, message);
            }
        }

        /// <summary>
        /// Forward message as specified in the given subscription.
        /// </summary>
        /// <param name="subscription">
        /// The subscription.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        private void ForwardMessage(RealtimeSubscription subscription, HL7Message message)
        {
            // Example routing key template: "HL7.{MessageType}.{EventType}"
            var exchangeName = subscription.Name;
            var exchangeType = "topic";
            var routingKey = BuildRoutingKey(subscription, message);
            var serializedMessaged = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(serializedMessaged);

            using (var connection = this.connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchangeName, exchangeType);
                channel.BasicPublish(exchangeName, routingKey, null, body);
            }
        }
    }
}