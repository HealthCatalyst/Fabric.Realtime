namespace Fabric.Realtime.Domain.Models
{
    using System;

    public class HL7Message : Message
    {
        public string ExternalPatientID { get; set; }

        public string InternalPatientID { get; set; }

        public DateTime? MessageDate { get; set; }

        public string MessageEvent { get; set; }

        public string MessageType { get; set; }

        public string ReceivingApplication { get; set; }

        public string SendingApplication { get; set; }
    }
}