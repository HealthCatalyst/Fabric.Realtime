namespace Subscribe
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using RabbitMQ.Client.MessagePatterns;

    /// <inheritdoc />
    public class Subscriber : ISubscriber
    {
        /// <inheritdoc />
        /// <remarks>
        /// This implementation supports automatic reconnection to the AMQP broker.
        /// in the event of network failures and AMQP broker server restarts.
        /// </remarks>
        public async Task RunAsync(SubscriptionDefinition subscription, CancellationToken cancellationToken, Func<SubscriberMessage, bool> handler)
        {
            while (true)
            {
                try
                {
                    await this.ReceiveMessagesAsync(subscription, cancellationToken, handler);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }

                Console.WriteLine("Waiting for connection...");
                await Task.Delay(5000, cancellationToken);
            }
            // ReSharper disable once FunctionNeverReturns
        }

        /// <summary>
        /// Creates a connection to the AMQP broker to receive messages from a message queue.
        /// </summary>
        /// <param name="subscription">
        /// The subscription.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <param name="handler">
        /// The handler.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task ReceiveMessagesAsync(SubscriptionDefinition subscription, CancellationToken cancellationToken, Func<SubscriberMessage, bool> handler)
        {
            Console.WriteLine($"Connection to the AMQP broker at '{subscription.Broker.Host}'");
            var factory = new ConnectionFactory { HostName = subscription.Broker.Host };
            using (var connection = factory.CreateConnection())
            using (var amqpModel = connection.CreateModel())
            {
                Console.WriteLine($"Declaring exchange '{subscription.Exchange.Name}' (type={subscription.Exchange.Type})");

                amqpModel.ExchangeDeclare(
                    subscription.Exchange.Name,
                    GetExchangeTypeString(subscription.Exchange.Type),
                    subscription.Exchange.IsDurable);

                var queueName = amqpModel.QueueDeclare(
                    subscription.Queue.Name,
                    subscription.Queue.IsDurable,
                    false,
                    subscription.Queue.IsAutoDelete);
           
                Console.WriteLine($"Binding to exchange using routing key'{subscription.RoutingKey}'");
                amqpModel.QueueBind(queueName, subscription.Exchange.Name, subscription.RoutingKey);

                Console.WriteLine($"Waiting for messages on queue '{queueName}'.");
                await MessageLoopAsync(cancellationToken, handler, amqpModel, queueName);
            }
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
        private static async Task MessageLoopAsync(
            CancellationToken cancellationToken,
            Func<SubscriberMessage, bool> handler,
            IModel amqpModel,
            QueueDeclareOk queueName)
        {
            var sub = new Subscription(amqpModel, queueName);

            while (true)
            {
                var response = sub.Next(10, out BasicDeliverEventArgs ea);
                if (response == false)
                {
                    if (amqpModel.IsClosed)
                    {
                        Console.WriteLine("Channel is closed.");
                        return;
                    }

                    continue;
                }

                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Console.WriteLine($"Received {routingKey}: {message}");

                // Forward to handler delegate
                var handlerResult = handler.Invoke(BuildSubscriberMessage(ea));

                // Acknowledge message on success
                if (handlerResult)
                {
                    sub.Ack(ea);
                }

                await Task.Delay(100, cancellationToken);
            }
        }

        /// <summary>
        /// Builds a new subscriber message.
        /// </summary>
        /// <param name="ea">
        /// The <see cref="BasicDeliverEventArgs"/> from the AMQP broker.
        /// </param>
        /// <returns>
        /// The <see cref="SubscriberMessage"/>.
        /// </returns>
        private static SubscriberMessage BuildSubscriberMessage(BasicDeliverEventArgs ea)
        {
            return new SubscriberMessage
                       {
                           Body = ea.Body,
                           ConsumerTag = ea.ConsumerTag,
                           DeliveryTag = ea.DeliveryTag,
                           Exchange = ea.Exchange,
                           Redelivered = ea.Redelivered,
                           RoutingKey = ea.RoutingKey
                       };
        }

        /// <summary>
        /// The get exchange type string.
        /// </summary>
        /// <param name="exchangeType">
        /// The exchange type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetExchangeTypeString(ExchangeType exchangeType)
        {
            return exchangeType.ToString().ToLower();
        }
    }
}
