namespace Fabric.Realtime.Domain
{
    using System;

    /// <summary>
    /// The Message interface.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        long Id { get; set; }

        /// <summary>
        /// Gets or sets the message hash.
        /// </summary>
        string MessageHash { get; set; }

        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the protocol version.
        /// </summary>
        string ProtocolVersion { get; set; }

        /// <summary>
        /// Gets or sets the raw message.
        /// </summary>
        string RawMessage { get; set; }

        /// <summary>
        /// Gets or sets the transmission receipt time.
        /// </summary>
        DateTimeOffset TransmissionReceiptTime { get; set; }
    }
}