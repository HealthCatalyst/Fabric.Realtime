namespace Subscribe
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using EntryPoint;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using RabbitMQ.Client.MessagePatterns;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class Program
    {
        private static CancellationToken cancellationToken = new CancellationToken();

        public static void Main(string[] args)
        {
            var arguments = ParseCommandLineArgs(args);

            var subscriber = new Subscriber();
            subscriber.RunAsync(
                arguments,
                cancellationToken,
                e =>
                    {
                        Console.WriteLine(e.DeliveryTag);
                        return true;
                    }).Wait();
        }

        private static SubscriptionDefinition ParseCommandLineArgs(string[] args)
        {
            if (args.Length == 0)
            {
                var helpText = Cli.GetHelp<SubscribeArguments>();
                Console.WriteLine(helpText);
                Environment.Exit(0);
            }

            // Parse the command line arguments
            var arguments = Cli.Parse<SubscribeArguments>(args);

            return new SubscriptionDefinition
                       {
                           Broker =
                               new BrokerHostConfiguration { Host = arguments.RabbitHostName },
                           Exchange =
                               new ExchangeDefinition
                                   {
                                       Name = arguments.ExchangeName,
                                       Type = ExchangeType.Topic,
                                       IsDurable = true,
                                       IsAutoDelete = false
                                   },
                           Queue = new QueueDefinition
                                       {
                                           Name =
                                               $"{arguments.ExchangeName}-{Guid.NewGuid().ToString("N").ToLower()}",
                                           IsDurable = false,
                                           IsAutoDelete = true
                                       },
                           RoutingKey = arguments.RoutingKey
                       };
        }

        ////private static async Task RunAsync(SubscribeArguments options)
        ////{
        ////    while (true)
        ////    {
        ////        try
        ////        {
        ////            await ReceiveMessagesAsync(options);
        ////        }
        ////        catch (Exception exception)
        ////        {
        ////            Console.WriteLine(exception.Message);
        ////        }

        ////        Console.WriteLine("Waiting for connection...");
        ////        await Task.Delay(5000);
        ////    }
        ////    // ReSharper disable once FunctionNeverReturns
        ////}

        ////private static async Task ReceiveMessagesAsync(SubscribeArguments options)
        ////{
        ////    Console.WriteLine($"Connection to RabbitMQ at '{options.RabbitHostName}'");
        ////    var factory = new ConnectionFactory { HostName = options.RabbitHostName };
        ////    using (var connection = factory.CreateConnection())
        ////    using (var amqpModel = connection.CreateModel())
        ////    {
        ////        Console.WriteLine($"Declaring exchange '{options.ExchangeName}' (type={options.ExchangeType})");
        ////        amqpModel.ExchangeDeclare(options.ExchangeName, options.ExchangeType, durable: true);
        ////        var queueName = amqpModel.QueueDeclare().QueueName;

        ////        Console.WriteLine($"Waiting for messages on queue '{queueName}'.");

        ////        Console.WriteLine($"Binding to exchange using routing key'{options.RoutingKey}'");

        ////        amqpModel.QueueBind(queueName, options.ExchangeName, options.RoutingKey);
        ////        Subscription sub = new Subscription(amqpModel, queueName);

        ////        while (true)
        ////        {
        ////            var response = sub.Next(10, out BasicDeliverEventArgs ea);
        ////            if (response == false)
        ////            {
        ////                if (amqpModel.IsClosed)
        ////                {
        ////                    Console.WriteLine("Channel is closed.");
        ////                    return;
        ////                }

        ////                continue;
        ////            }

        ////            var body = ea.Body;
        ////            var message = Encoding.UTF8.GetString(body);
        ////            var routingKey = ea.RoutingKey;
        ////            Console.WriteLine($"Received {routingKey}: {message}");
        ////            sub.Ack(ea);

        ////            await Task.Delay(100);
        ////        }
        ////    }
        ////}
    }
}
