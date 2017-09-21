namespace Fabric.Realtime.Engine.EventBus.Models
{
    /// <summary>
    /// HL7 interface engine message.
    /// </summary>
    public class HL7InterfaceEngineMessage : InterfaceEngineMessage
    {
        /// <summary>
        /// Gets or sets the external patient id.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string ExternalPatientID { get; set; }

        /// <summary>
        /// Gets or sets the internal patient id.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string InternalPatientID { get; set; }

        /// <summary>
        /// Gets or sets the message date.
        /// </summary>
        public string MessageDate { get; set; }

        /// <summary>
        /// Gets or sets the message event.
        /// </summary>
        public string MessageEvent { get; set; }

        /// <summary>
        /// Gets or sets the message type.
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the receiving application.
        /// </summary>
        public string ReceivingApplication { get; set; }

        /// <summary>
        /// Gets or sets the sending application.
        /// </summary>
        public string SendingApplication { get; set; }
    }
}