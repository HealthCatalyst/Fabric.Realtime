namespace Fabric.Realtime.EventBus.Implementations
{
    using System;
    using System.Text;

    using Fabric.Realtime.EventBus.Models;
    using Fabric.Realtime.Handlers;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class InterfaceEngineQueueService : IDisposable, IInitializable
    {
        private IModel _channel;

        private IConnection _connection;

        private EventingBasicConsumer _consumer;

        private readonly IInterfaceEngineEventHandler _eventHandler;

        private readonly MessageBrokerExchangeClient _interfaceEngine;

        public InterfaceEngineQueueService(
            MessageBrokerExchangeClient interfaceEngine,
            IInterfaceEngineEventHandler eventHandler)
        {
            this._interfaceEngine = interfaceEngine;
            this._eventHandler = eventHandler;
        }

        public void Dispose()
        {
            this._channel.Dispose();
            this._connection.Dispose();
        }

        public void Initialize()
        {
            var factory = new ConnectionFactory
                              {
                                  HostName = this._interfaceEngine.HostName,
                                  Port = this._interfaceEngine.Port
                              };
            this._connection = factory.CreateConnection();
            this._channel = this._connection.CreateModel();

            this._channel.ExchangeDeclare(this._interfaceEngine.Exchange, "direct");

            this._channel.QueueDeclare(this._interfaceEngine.Queue, true, false, false, null);

            this._channel.BasicQos(0, 1, false);

            this._channel.QueueBind(
                this._interfaceEngine.Queue,
                this._interfaceEngine.Exchange,
                this._interfaceEngine.RoutingKey);

            this._consumer = new EventingBasicConsumer(this._channel);
            this._consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var task = this._eventHandler.HandleMessage(message);
                    var success = task.IsCompleted;
                    if (success) this._channel.BasicAck(ea.DeliveryTag, false);
                };

            this._channel.BasicConsume(this._interfaceEngine.Queue, false, this._consumer);
        }
    }
}