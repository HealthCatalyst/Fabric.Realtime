namespace Fabric.Realtime.Engine.Transformers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Fabric.Realtime.Domain;
    using Fabric.Realtime.Engine.EventBus.Models;

    /// <summary>
    /// The interface engine message transformer.
    /// </summary>
    public class InterfaceEngineMessageTransformer : IInterfaceEngineMessageTransformer
    {
        /// <summary>
        /// The epoch date time in Coordinated Universal Time (UTC).
        /// </summary>
        private readonly DateTime epochDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Transforms a InterfaceEngineMessage to an IMessage type.
        /// </summary>
        /// <param name="interfaceEngineMessage">
        /// The interface engine message.
        /// </param>
        /// <returns>
        /// The <see cref="IMessage"/>.
        /// </returns>
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
                MessageDate = ParseMessageDate(hl7SourceMessage.MessageDate),
                ExternalPatientID = hl7SourceMessage.ExternalPatientID,
                InternalPatientID = hl7SourceMessage.InternalPatientID,
                MessageType = hl7SourceMessage.MessageType,
                EventType = hl7SourceMessage.MessageEvent,
                SendingApplication = hl7SourceMessage.SendingApplication,
                ReceivingApplication = hl7SourceMessage.ReceivingApplication,
                RawMessage = interfaceEngineMessage.RawMessage,
                XmlMessage = interfaceEngineMessage.XmlMessage,
                TransmissionReceiptTime =
                    this.epochDateTime.AddMilliseconds(interfaceEngineMessage.TransmissionReceiptTimeInMillis)
            };
        }

        /// <summary>
        /// Parses the message date.
        /// </summary>
        /// <param name="dateString">
        /// The source message date as a string.
        /// </param>
        /// <returns>
        /// The <see cref="DateTimeOffset"/> or null if unable to parse the given date.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed. Suppression is OK here.")]
        private static DateTimeOffset? ParseMessageDate(string dateString)
        {
            return DateTime.TryParseExact(
                       dateString,
                       "yyyyMMddHHmmssff",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out var dt)
                       ? new DateTimeOffset(dt)
                       : (DateTimeOffset?)null;
        }
    }
}