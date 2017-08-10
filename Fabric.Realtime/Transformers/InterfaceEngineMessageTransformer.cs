namespace Fabric.Realtime.Transformers
{
    using System;

    using Fabric.Realtime.Domain.Models;
    using Fabric.Realtime.EventBus.Models;

    public class InterfaceEngineMessageTransformer : IInterfaceEngineMessageTransformer
    {
        public Message Transform(InterfaceEngineMessage interfaceEngineMessage)
        {
            var Message = new Message();
            Message.Protocol = (MessageProtocol)Enum.Parse(typeof(MessageProtocol), interfaceEngineMessage.Protocol);
            Message.Version = interfaceEngineMessage.Version;
            Message.MessageHash = interfaceEngineMessage.MessageHash;
            Message.RawMessage = interfaceEngineMessage.RawMessage;
            Message.XmlMessage = interfaceEngineMessage.XmlMessage;
            Message.TransmissionReceiptTime = new DateTime(interfaceEngineMessage.TransmissionReceiptTimeInMillis);
            return Message;
        }
    }
}