namespace Fabric.Realtime.Engine.Configuration
{
    public interface IRealtimeConfiguration
    {
        DatabaseSettings DatabaseSettings { get; set; }
        InterfaceEngineSettings InterfaceEngineSettings { get; set; }
        MessageBrokerSettings MessageBrokerSettings { get; set; }
    }
}