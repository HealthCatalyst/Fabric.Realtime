namespace Fabric.Realtime.Data.Models
{
    using System;

    public class HL7Message : IMessage
    {
        public long Id { get; set; }

        public string MessageHash { get; set; }

        public string Protocol { get; set; }

        public string ProtocolVersion { get; set; }

        public string MessageType { get; set; }

        public string EventType { get; set; }

        public DateTimeOffset? MessageDate { get; set; }

        public DateTimeOffset TransmissionReceiptTime { get; set; }

        public string SendingApplication { get; set; }

        public string SendingFacility { get; set; }

        public string ReceivingApplication { get; set; }

        public string ReceivingFacility { get; set; }

        public string ExternalPatientID { get; set; }

        public string InternalPatientID { get; set; }

        public string MessageControlID { get; set; }

        public string ProcessingID { get; set; }

        public string RawMessage { get; set; }

        public string XmlMessage { get; set; }
    }

}
