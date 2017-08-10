namespace Fabric.Realtime.EventBus.Implementations
{
    using System.Text;

    using Fabric.Realtime.EventBus.Models;
    using Fabric.Realtime.Handlers;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class RabbitMQQueueService : IQueueService
    {
        IModel _channel;

        IConnection _connection;

        EventingBasicConsumer _consumer;

        IInterfaceEngineEventHandler _eventHandler;

        string _messageExchange;

        string _messageQueue;

        string _primaryHost;

        string _routingKey;

        string _secondaryHost;

        public RabbitMQQueueService(
            string primaryHost,
            string secondaryHost,
            string messageExchange,
            string messageQueue,
            string routingKey,
            IInterfaceEngineEventHandler eventHandler)
        {
            this._primaryHost = primaryHost;
            this._secondaryHost = secondaryHost;
            this._messageExchange = messageExchange;
            this._messageQueue = messageQueue;
            this._routingKey = routingKey;
            this._eventHandler = eventHandler;

            var factory = new ConnectionFactory() { HostName = this._primaryHost };
            this._connection = factory.CreateConnection();
            this._channel = this._connection.CreateModel();

            this._channel.ExchangeDeclare(exchange: this._messageExchange, type: "direct");

            this._channel.QueueDeclare(
                queue: this._messageQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            this._channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            this._channel.QueueBind(
                queue: this._messageQueue,
                exchange: this._messageExchange,
                routingKey: this._routingKey);

            this._consumer = new EventingBasicConsumer(this._channel);
            this._consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var task = this._eventHandler.HandleMessage(message);
                    var success = task.IsCompleted;
                    if (success)
                    {
                        this._channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                };

            this._channel.BasicConsume(queue: this._messageQueue, autoAck: false, consumer: this._consumer);
        }

        public void Dispose()
        {
            this._channel.Dispose();
            this._connection.Dispose();
        }
    }
}