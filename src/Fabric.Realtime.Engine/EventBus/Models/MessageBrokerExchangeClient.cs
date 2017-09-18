namespace Fabric.Realtime.Engine.EventBus.Models
{
    public class MessageBrokerExchangeClient : MessageBrokerExchange
    {
        public MessageBrokerExchangeClient(string hostName, int port, string exchange, string queue, string routingKey)
            : base(hostName, port, exchange)
        {
            this.Queue = queue;
            this.RoutingKey = routingKey;
        }

        public string Queue { get; set; }

        public string RoutingKey { get; set; }
    }
}