namespace Fabric.Realtime.Engine.EventBus.Models
{
    /// <summary>
    /// The interface engine message.
    /// </summary>
    public class InterfaceEngineMessage
    {
        /// <summary>
        /// Gets or sets the message hash.
        /// </summary>
        public string MessageHash { get; set; }

        /// <summary>
        /// Gets or sets the protocol (e.g. HL7, X12, etc.).
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the raw source message.
        /// </summary>
        /// <remarks>
        /// This will be the content of the original raw source 
        /// message (e.g. HL7, X12, etc.) that was received.
        /// </remarks>
        public string RawMessage { get; set; }

        /// <summary>
        /// Gets or sets the transmission receipt time in millis.
        /// </summary>
        public long TransmissionReceiptTimeInMillis { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the XML message, if available.
        /// </summary>
        public string XmlMessage { get; set; }
    }
}