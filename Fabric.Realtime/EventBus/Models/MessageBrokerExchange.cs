namespace Fabric.Realtime.EventBus.Models
{
    public class MessageBrokerExchange : MessageBroker
    {
        public MessageBrokerExchange(string hostName, int port, string exchange)
            : base(hostName, port)
        {
            this.Exchange = exchange;
        }

        public string Exchange { get; set; }
    }
}