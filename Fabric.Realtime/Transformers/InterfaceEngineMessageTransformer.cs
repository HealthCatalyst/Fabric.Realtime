namespace Fabric.Realtime.Transformers
{
    using System;
    using System.Globalization;

    using Fabric.Realtime.Domain.Models;
    using Fabric.Realtime.EventBus.Models;

    public class InterfaceEngineMessageTransformer : IInterfaceEngineMessageTransformer
    {
        private readonly DateTime epochDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public Message Transform(InterfaceEngineMessage interfaceEngineMessage)
        {
            Message message;
            if (interfaceEngineMessage.Protocol.Equals(MessageProtocol.HL7.ToString()))
            {
                var hl7Message = new HL7Message();
                hl7Message.Protocol =
                    (MessageProtocol)Enum.Parse(typeof(MessageProtocol), interfaceEngineMessage.Protocol);
                hl7Message.ProtocolVersion = interfaceEngineMessage.Version;
                hl7Message.MessageHash = interfaceEngineMessage.MessageHash;
                hl7Message.MessageDate = DateTime.ParseExact(
                    ((HL7InterfaceEngineMessage)interfaceEngineMessage).MessageDate,
                    "yyyyMMddHHmmssff",
                    CultureInfo.InvariantCulture);
                hl7Message.ExternalPatientID = ((HL7InterfaceEngineMessage)interfaceEngineMessage).ExternalPatientID;
                hl7Message.InternalPatientID = ((HL7InterfaceEngineMessage)interfaceEngineMessage).InternalPatientID;
                hl7Message.MessageEvent = ((HL7InterfaceEngineMessage)interfaceEngineMessage).MessageEvent;
                hl7Message.MessageType = ((HL7InterfaceEngineMessage)interfaceEngineMessage).MessageType;
                hl7Message.SendingApplication = ((HL7InterfaceEngineMessage)interfaceEngineMessage).SendingApplication;
                hl7Message.ReceivingApplication = ((HL7InterfaceEngineMessage)interfaceEngineMessage)
                    .ReceivingApplication;
                hl7Message.RawMessage = interfaceEngineMessage.RawMessage;
                hl7Message.XmlMessage = interfaceEngineMessage.XmlMessage;
                hl7Message.TransmissionReceiptTime =
                    this.epochDateTimeUtc.AddMilliseconds(interfaceEngineMessage.TransmissionReceiptTimeInMillis);
                message = hl7Message;
            }
            else
            {
                throw new Exception("Non-HL7 protocols are not supported at this time.");
            }

            return message;
        }
    }
}