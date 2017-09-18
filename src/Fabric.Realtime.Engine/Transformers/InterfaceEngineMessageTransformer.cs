namespace Fabric.Realtime.Engine.Transformers
{
    using System;
    using System.Globalization;

    using Fabric.Realtime.Data.Models;
    using Fabric.Realtime.Engine.EventBus.Models;

    public class InterfaceEngineMessageTransformer : IInterfaceEngineMessageTransformer
    {
        private readonly DateTime epochDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public IMessage Transform(InterfaceEngineMessage interfaceEngineMessage)
        {
            // ReSharper disable once UsePatternMatching
            var hl7SourceMessage = interfaceEngineMessage as HL7InterfaceEngineMessage;
            if (hl7SourceMessage == null)
            {
                throw new Exception("Non-HL7 protocols are not supported at this time.");
            }

            return new HL7Message
            {
                Protocol = interfaceEngineMessage.Protocol,
                ProtocolVersion = interfaceEngineMessage.Version,
                MessageHash = interfaceEngineMessage.MessageHash,
                MessageDate =
                    DateTime.ParseExact(
                        hl7SourceMessage.MessageDate,
                        "yyyyMMddHHmmssff",
                        CultureInfo.InvariantCulture),
                ExternalPatientID = hl7SourceMessage.ExternalPatientID,
                InternalPatientID = hl7SourceMessage.InternalPatientID,
                MessageType = hl7SourceMessage.MessageType,
                EventType = hl7SourceMessage.MessageEvent,
                SendingApplication = hl7SourceMessage.SendingApplication,
                ReceivingApplication = hl7SourceMessage.ReceivingApplication,
                RawMessage = interfaceEngineMessage.RawMessage,
                XmlMessage = interfaceEngineMessage.XmlMessage,
                TransmissionReceiptTime =
                    this.epochDateTimeUtc.AddMilliseconds(interfaceEngineMessage.TransmissionReceiptTimeInMillis)
            };
        }
    }
}