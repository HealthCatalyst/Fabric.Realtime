namespace Subscribe
{
    using System;

    using EntryPoint;

    /// <summary>
    /// The subscribe command line arguments.
    /// </summary>
    public class SubscribeCliArguments : BaseCliArguments
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeCliArguments"/> class.
        /// </summary>
        public SubscribeCliArguments()
            : base("Subscribe")
        {
        }

        /// <summary>
        /// Gets or sets the RabbitMQ server host name parameter.
        /// </summary>
        [OptionParameter("rabbit", 'r')]
        [Required]
        public string RabbitHostName { get; set; }

        /// <summary>
        /// Gets or sets the exchange name option parameter.
        /// </summary>
        [OptionParameter("exchange-name", 'e')]
        [Required]
        public string ExchangeName { get; set; }

        /// <summary>
        /// Gets or sets the exchange type option parameter.
        /// </summary>
        [OptionParameter("exchange-type", 't')]
        [Required]
        public string ExchangeType { get; set; }

        /// <summary>
        /// Gets or sets the exchange type option parameter.
        /// </summary>
        [OptionParameter("routing-key", 'k')]
        [Required]
        public string RoutingKey { get; set; }

        /// <summary>
        /// When the user invokes --help/-h a default handler will take over
        /// You can define your own behavior by overriding this Virtual method.
        /// </summary>
        /// <param name="helpText">
        /// The help text.
        /// </param>
        public override void OnHelpInvoked(string helpText)
        {
            Console.WriteLine(helpText);
            Environment.Exit(0);
        }
    }
}