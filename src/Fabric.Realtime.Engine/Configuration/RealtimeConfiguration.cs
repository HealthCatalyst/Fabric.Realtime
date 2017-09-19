namespace Fabric.Realtime.Engine.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The realtime configuration implementation.
    /// </summary>
    public class RealtimeConfiguration : IRealtimeConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RealtimeConfiguration"/> class.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        public RealtimeConfiguration(IConfiguration config)
        {
            DatabaseSettings = config.GetSection("Database").Get<DatabaseSettings>();
            MessageBrokerSettings = config.GetSection("Broker").Get<MessageBrokerSettings>();
            InterfaceEngineSettings = config.GetSection("InterfaceEngine").Get<InterfaceEngineSettings>();
        }

        /// <summary>
        /// Gets or sets the database settings.
        /// </summary>
        public DatabaseSettings DatabaseSettings { get; set; }

        /// <summary>
        /// Gets or sets the message broker settings.
        /// </summary>
        public MessageBrokerSettings MessageBrokerSettings { get; set; }

        /// <summary>
        /// Gets or sets the interface engine settings.
        /// </summary>
        public InterfaceEngineSettings InterfaceEngineSettings { get; set; }

        /// <summary>
        /// The in memory configuration.
        /// </summary>
        private static Dictionary<string, string> InMemoryConfiguration => new Dictionary<string, string>
        {
            {
                "Database:ConnectionString",
                "Server=(local);Database=FabricRealtime;Trusted_Connection=True;MultipleActiveResultSets=true"
            },
            { "Broker:AmqpUri", "amqp://guest:guest@localhost:5672" },
            { "InterfaceEngine:ExchangeName", "fabric.interfaceengine" },
            { "InterfaceEngine:QueueName", "fabric.interfaceengine.queue" },
            { "InterfaceEngine:RoutingKey", "inbound" }
        };

        /// <summary>
        /// Builds the configuration root.
        /// </summary>
        /// <returns>
        /// The <see cref="IConfigurationRoot"/>.
        /// </returns>
        public static IConfigurationRoot BuildConfigurationRoot()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(InMemoryConfiguration)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddCommandLine(Environment.GetCommandLineArgs().Skip(1).ToArray());

            return configBuilder.Build();
        }
    }
}
