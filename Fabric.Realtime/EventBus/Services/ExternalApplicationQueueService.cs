namespace Fabric.Realtime.EventBus.Services
{
    using System;
    using System.Text;

    using Fabric.Realtime.Domain.Models;
    using Fabric.Realtime.EventBus.Models;

    using Newtonsoft.Json;

    using RabbitMQ.Client;

    public class ExternalApplicationQueueService : IDisposable, IInitializable
    {
        private IConnection _connection;

        private readonly MessageBrokerExchange _messageBrokerExchange;

        public ExternalApplicationQueueService(MessageBrokerExchange messageBrokerExchange)
        {
            this._messageBrokerExchange = messageBrokerExchange;
        }

        public void Dispose()
        {
            this._connection.Dispose();
        }

        public void ForwardMessage(string routingKey, HL7Message message)
        {
            var serializedMessaged = JsonConvert.SerializeObject(message);

            using (var channel = this._connection.CreateModel())
            {
                channel.ExchangeDeclare(this._messageBrokerExchange.Exchange, "direct");

                channel.BasicPublish(
                    this._messageBrokerExchange.Exchange,
                    routingKey,
                    null,
                    Encoding.UTF8.GetBytes(serializedMessaged));
            }
        }

        public void Initialize()
        {
            var factory = new ConnectionFactory
                              {
                                  HostName = this._messageBrokerExchange.HostName,
                                  Port = this._messageBrokerExchange.Port
                              };
            this._connection = factory.CreateConnection();
        }
    }
}