namespace Subscribe
{
    using System;

    /// <summary>
    /// The queue definition.
    /// </summary>
    public class QueueDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueDefinition"/> class.
        /// </summary>
        public QueueDefinition()
        {
            this.IsDurable = false;
            this.IsAutoDelete = true;
        }

        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is durable.
        /// </summary>
        public bool IsDurable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is auto delete.
        /// </summary>
        public bool IsAutoDelete { get; set; }

        /// <summary>
        /// Gets or sets the length of time a message published to a queue can 
        /// live before it is discarded.
        /// </summary>
        public TimeSpan? MessageTimeToLive { get; set; }

        /// <summary>
        /// Gets or sets the length of time a queue can be unused for before it 
        /// is automatically deleted.
        /// </summary>
        public TimeSpan? Expiration { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of (ready) messages a queue can contain 
        /// before it starts to drop them from its head.
        /// </summary>
        public uint? MaxMessageCount { get; set; }

        /// <summary>
        /// Gets or sets the optional name of an exchange to which messages 
        /// will be republished if they are rejected or expire.
        /// </summary>
        public string DeadLetterExchange { get; set; }
    }
}