namespace Fabric.Realtime.EventBus.Models
{
    public class HL7InterfaceEngineMessage : InterfaceEngineMessage
    {
        public HL7InterfaceEngineMessage(
            string protocol,
            string version,
            string messageHash,
            long transmissionReceiptTimeInMillis,
            string rawMessage,
            string xmlMessage,
            string sendingApplication,
            string receivingApplication,
            string internalPatientId,
            string externalPatientId,
            string messageDate,
            string messageType,
            string messageEvent)
            : base(protocol, version, messageHash, transmissionReceiptTimeInMillis, rawMessage, xmlMessage)
        {
            this.SendingApplication = sendingApplication;
            this.ReceivingApplication = receivingApplication;
            this.InternalPatientID = internalPatientId;
            this.ExternalPatientID = externalPatientId;
            this.MessageDate = messageDate;
            this.MessageType = messageType;
            this.MessageEvent = messageEvent;
        }

        public string ExternalPatientID { get; set; }

        public string InternalPatientID { get; set; }

        public string MessageDate { get; set; }

        public string MessageEvent { get; set; }

        public string MessageType { get; set; }

        public string ReceivingApplication { get; set; }

        public string SendingApplication { get; set; }
    }
}