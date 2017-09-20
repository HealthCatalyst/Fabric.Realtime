namespace Fabric.Realtime.Engine.EventBus.Services
{
    using System;
    using System.Text;

    using Fabric.Realtime.Domain;
    using Fabric.Realtime.Engine.EventBus.Models;

    using Newtonsoft.Json;

    using RabbitMQ.Client;

    public class ExternalApplicationQueueService : IDisposable, IInitializable
    {
        private IConnection connection;

        private readonly MessageBrokerExchange messageBrokerExchange;

        public ExternalApplicationQueueService(MessageBrokerExchange messageBrokerExchange)
        {
            this.messageBrokerExchange = messageBrokerExchange;
        }

        public void Dispose()
        {
            this.connection.Dispose();
        }

        public void ForwardMessage(string routingKey, HL7Message message)
        {
            var serializedMessaged = JsonConvert.SerializeObject(message);

            using (var channel = this.connection.CreateModel())
            {
                channel.ExchangeDeclare(this.messageBrokerExchange.Exchange, "direct");

                channel.BasicPublish(
                    this.messageBrokerExchange.Exchange,
                    routingKey,
                    null,
                    Encoding.UTF8.GetBytes(serializedMessaged));
            }
        }

        public void Initialize()
        {
            var factory = new ConnectionFactory
                              {
                                  HostName = this.messageBrokerExchange.HostName,
                                  Port = this.messageBrokerExchange.Port
                              };
            this.connection = factory.CreateConnection();
        }
    }
}