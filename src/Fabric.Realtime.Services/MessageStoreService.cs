namespace Fabric.Realtime.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Fabric.Realtime.Core.Utils;
    using Fabric.Realtime.Data.Stores;
    using Fabric.Realtime.Domain;

    using Microsoft.EntityFrameworkCore;

    /// <inheritdoc />
    public class MessageStoreService : IMessageStoreService
    {
        /// <summary>
        /// The database context.
        /// </summary>
        private readonly RealtimeContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageStoreService"/> class.
        /// </summary>
        /// <param name="context">
        /// The database context.
        /// </param>
        public MessageStoreService(RealtimeContext context)
        {
            Guard.ArgumentNotNull(context, nameof(context));
            this.context = context;
        }

        /// <inheritdoc />
        public void Add(HL7Message message)
        {
            Guard.ArgumentNotNull(message, nameof(message));
            this.context.HL7Messages.Add(message);
            this.context.SaveChanges();
        }

        /// <inheritdoc />
        public HL7Message FindById(long id)
        {
            return this.context.HL7Messages.AsNoTracking().FirstOrDefault(m => m.Id == id);
        }

        /// <inheritdoc />
        public IEnumerable<HL7Message> GetAll()
        {
            return this.context.HL7Messages.AsNoTracking().ToList();
        }

        /// <inheritdoc />
        public IEnumerable<HL7Message> FindByTimeRange(DateTimeOffset startTime, DateTimeOffset endTime)
        {
            throw new NotImplementedException();
        }
    }
}