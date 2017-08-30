namespace Fabric.Realtime.Domain.Models
{
    using System;
    using System.Collections;

    public class Message
    {
        public Message()
        {
        }

        public int Id { get; set; }

        public string MessageHash { get; set; }

        public MessageProtocol Protocol { get; set; }

        public string RawMessage { get; set; }

        public DateTime TransmissionReceiptTime { get; set; }

        public string ProtocolVersion { get; set; }

        public string XmlMessage { get; set; }
    }

    public enum MessageProtocol
    {
        HL7,

        X12
    }
}