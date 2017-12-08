namespace Subscribe
{
    /// <summary>
    /// The exchange type.
    /// </summary>
    public enum ExchangeType
    {
        /// <summary>
        /// A direct exchange.
        /// </summary>
        Direct,

        /// <summary>
        /// A header exchange.
        /// </summary>
        Headers,

        /// <summary>
        /// A fanout exchange.
        /// </summary>
        Fanout,

        /// <summary>
        /// A topic exchange
        /// </summary>
        Topic
    }

    /// <summary>
    /// The exchange definition.
    /// </summary>
    public class ExchangeDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeDefinition"/> class.
        /// </summary>
        public ExchangeDefinition()
        {
            this.IsDurable = false;
            this.IsAutoDelete = true;
            this.Type = ExchangeType.Topic;
        }

        /// <summary>
        /// Gets or sets the name of the exchange.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of exchange.
        /// </summary>
        public ExchangeType Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the exchange is durable.
        /// </summary>
        public bool IsDurable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the exchange is auto delete.
        /// </summary>
        public bool IsAutoDelete { get; set; }
    }
}