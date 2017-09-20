namespace Fabric.Realtime.Services
{
    using System;
    using System.Collections.Generic;

    using Fabric.Realtime.Domain;

    /// <summary>
    /// Defines the message storage service.
    /// </summary>
    public interface IMessageStoreService
    {
        /// <summary>
        /// Add a new message to the data store.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        void Add(HL7Message message);

        /// <summary>
        /// Finds a message by the given identifier.
        /// </summary>
        /// <param name="id">
        /// The unique message identifier.
        /// </param>
        /// <returns>
        /// The <see cref="HL7Message"/>, or null if not found.
        /// </returns>
        HL7Message FindById(long id);

        /// <summary>
        /// Finds all messages in the given time range.
        /// </summary>
        /// <param name="startTime">
        /// The (inclusive) start time.
        /// </param>
        /// <param name="endTime">
        /// The (inclusive) end time.
        /// </param>
        /// <returns>
        /// An enumerable of the messages in the given time range.
        /// </returns>
        IEnumerable<HL7Message> FindByTimeRange(DateTimeOffset startTime, DateTimeOffset endTime);
    }
}
