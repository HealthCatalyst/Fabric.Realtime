namespace Fabric.Realtime.Engine.EventBus.Models
{
    public class MessageBroker
    {
        public MessageBroker(string hostName, int port)
        {
            this.HostName = hostName;
            this.Port = port;
        }

        public string HostName { get; set; }

        public int Port { get; set; }
    }
}