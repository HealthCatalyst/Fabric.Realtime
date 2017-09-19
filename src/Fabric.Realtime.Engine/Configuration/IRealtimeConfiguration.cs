namespace Fabric.Realtime.Engine.Configuration
{
    /// <summary>
    /// The RealtimeConfiguration interface.
    /// </summary>
    public interface IRealtimeConfiguration
    {
        /// <summary>
        /// Gets or sets the database settings.
        /// </summary>
        DatabaseSettings DatabaseSettings { get; set; }

        /// <summary>
        /// Gets or sets the interface engine settings.
        /// </summary>
        InterfaceEngineSettings InterfaceEngineSettings { get; set; }

        /// <summary>
        /// Gets or sets the message broker settings.
        /// </summary>
        MessageBrokerSettings MessageBrokerSettings { get; set; }
    }
}