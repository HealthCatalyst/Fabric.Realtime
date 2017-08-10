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

        public string MessageHash { get; private set; }

        public string Protocol { get; private set; }

        public string RawMessage { get; private set; }

        public long TransmissionReceiptTimeInMillis { get; private set; }

        public string Version { get; private set; }

        public string XmlMessage { get; private set; }
    }
}