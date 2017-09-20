namespace Fabric.Realtime.Engine.EventBus.Services
{
    using System;
    using System.Text;

    using Fabric.Realtime.Engine.EventBus.Models;
    using Fabric.Realtime.Engine.Handlers;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    /// <summary>
    /// Receives messages from the incoming queue.
    /// </summary>
    public class InterfaceEngineQueueService : IDisposable, IInitializable
    {
        private IModel channel;

        private IConnection connection;

        private EventingBasicConsumer consumer;

        private readonly IInterfaceEngineEventHandler eventHandler;

        private readonly MessageBrokerExchangeClient interfaceEngine;

        public InterfaceEngineQueueService(
            MessageBrokerExchangeClient interfaceEngine,
            IInterfaceEngineEventHandler eventHandler)
        {
            this.interfaceEngine = interfaceEngine;
            this.eventHandler = eventHandler;
        }

        public void Dispose()
        {
            this.channel.Dispose();
            this.connection.Dispose();
        }

        public void Initialize()
        {
            var factory = new ConnectionFactory
                              {
                                  HostName = this.interfaceEngine.HostName,
                                  Port = this.interfaceEngine.Port
                              };
            this.connection = factory.CreateConnection();
            this.channel = this.connection.CreateModel();

            this.channel.ExchangeDeclare(this.interfaceEngine.Exchange, "direct");

            this.channel.QueueDeclare(this.interfaceEngine.Queue, true, false, false, null);

            this.channel.BasicQos(0, 1, false);

            this.channel.QueueBind(
                this.interfaceEngine.Queue,
                this.interfaceEngine.Exchange,
                this.interfaceEngine.RoutingKey);

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

            this.channel.BasicConsume(this.interfaceEngine.Queue, false, this.consumer);
        }
    }
}