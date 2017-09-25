namespace Receive
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class Receive
    {
        private const string MqHostName = "localhost";
        private const string ExchangeName = "HL7-CAP";
        private const string ExchangeType = "topic";
        ////private const string ExchangeName = "fabric.interfaceengine";
        ////private const string QueueName = "fabric.interfaceengine";
        ////private const string ExchangeType = "direct";

        public static void Main(string[] args)
        {
            Console.WriteLine($"Connecting to RabbitMQ on host '{MqHostName}'");
            var factory = new ConnectionFactory { HostName = MqHostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType);
                var queueName = channel.QueueDeclare().QueueName;

                if (args.Length < 1)
                {
                    Console.Error.WriteLine("Usage: {0} [binding_key...]", Environment.GetCommandLineArgs()[0]);
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                    Environment.ExitCode = 1;
                    return;
                }

                foreach (var bindingKey in args)
                {
                    channel.QueueBind(queue: queueName, exchange: ExchangeName, routingKey: bindingKey);
                }

                Console.WriteLine($"Waiting for messages on queue '{queueName}'. To exit press CTRL+C");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;
                        Console.WriteLine($"Received {routingKey}: {message}");
                    };
                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
