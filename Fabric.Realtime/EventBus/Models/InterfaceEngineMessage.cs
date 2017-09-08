namespace Fabric.Realtime.EventBus.Models
{
    public class InterfaceEngineMessage
    {
        public InterfaceEngineMessage(
            string protocol,
            string version,
            string messageHash,
            long transmissionReceiptTimeInMillis,
            string rawMessage,
            string xmlMessage)
        {
            this.Protocol = protocol;
            this.Version = version;
            this.MessageHash = messageHash;
            this.TransmissionReceiptTimeInMillis = transmissionReceiptTimeInMillis;
            this.RawMessage = rawMessage;
            this.XmlMessage = xmlMessage;
        }

        public string MessageHash { get; }

        public string Protocol { get; }

        public string RawMessage { get; }

        public long TransmissionReceiptTimeInMillis { get; }

        public string Version { get; }

        public string XmlMessage { get; }
    }
}