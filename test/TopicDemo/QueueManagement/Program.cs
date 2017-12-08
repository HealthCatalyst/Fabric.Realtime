namespace QueueManagement
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class Program
    {
        private const string MqHostName = "localhost";
        private const string ExchangeName = "proto.queues";
        private const string ExchangeType = "topic";
       

        public static void Main(string[] args)
        {
            Console.WriteLine($"Connecting to RabbitMQ on host '{MqHostName}'");
            var factory = new ConnectionFactory { HostName = MqHostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var routingKey = "#.ADT.A01.#";
                var queueName = $"HL7 ADT Mart|{routingKey}";
                channel.ExchangeDeclare(ExchangeName,  ExchangeType);
                var result = channel.QueueDeclare(queueName, true, false, false);

                channel.QueueBind(queueName, ExchangeName, routingKey);

                Console.WriteLine($"Waiting for messages on queue '{queueName}'. To exit press CTRL+C");

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
