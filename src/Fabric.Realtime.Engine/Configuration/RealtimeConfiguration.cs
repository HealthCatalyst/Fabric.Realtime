using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Fabric.Realtime.Engine.Configuration
{
    public class RealtimeConfiguration : IRealtimeConfiguration
    {
        public RealtimeConfiguration(IConfiguration config)
        {
            this.DatabaseSettings = config.GetSection("Database").Get<DatabaseSettings>();
            this.MessageBrokerSettings = config.GetSection("Broker").Get<MessageBrokerSettings>();
            this.InterfaceEngineSettings = config.GetSection("InterfaceEngine").Get<InterfaceEngineSettings>();
        }

        public DatabaseSettings DatabaseSettings { get; set; }
        public MessageBrokerSettings MessageBrokerSettings { get; set; }
        public InterfaceEngineSettings InterfaceEngineSettings { get; set; }

        public static IConfigurationRoot BuildConfigurationRoot()
        {

            var configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(InMemoryConfiguration)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddCommandLine(Environment.GetCommandLineArgs().Skip(1).ToArray());

            return configBuilder.Build();
        }

        private static Dictionary<string, string> InMemoryConfiguration => new Dictionary<string, string>
        {
            {
                "Database:ConnectionString",
                "Server=(local);Database=FabricRealtime;Trusted_Connection=True;MultipleActiveResultSets=true"
            },
            {"Broker:AmqpUri", "amqp://guest:guest@localhost:5672"},
            {"InterfaceEngine:ExchangeName", "fabric.interfaceengine"},
            {"InterfaceEngine:QueueName", "fabric.interfaceengine.queue"},
            {"InterfaceEngine:RoutingKey", "inbound"}
        };
    }
}
