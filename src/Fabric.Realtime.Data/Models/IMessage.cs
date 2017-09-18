namespace Fabric.Realtime.Data.Models
{
    using System;

    public interface IMessage
    {
        long Id { get; set; }
        string MessageHash { get; set; }
        string Protocol { get; set; }
        string ProtocolVersion { get; set; }
        string RawMessage { get; set; }
        DateTimeOffset TransmissionReceiptTime { get; set; }
    }
}