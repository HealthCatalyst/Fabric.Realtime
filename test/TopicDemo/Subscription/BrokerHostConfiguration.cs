namespace Subscribe
{
    /// <summary>
    /// The message broker host configuration.
    /// </summary>
    public class BrokerHostConfiguration
    {
        /// <summary>
        /// The default port.
        /// </summary>
        public const int DefaultPort = 5672;

        /// <summary>
        /// The default TLS/SSL port.
        /// </summary>
        public const int DefaultSslPort = 5671;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrokerHostConfiguration"/> class.
        /// </summary>
        public BrokerHostConfiguration()
        {
            this.Port = DefaultPort;
            this.VirtualHost = "/";
            this.UserName = "guest";
            this.Password = "guest";
            ////Ssl = new SslOption();
        }

        /// <summary>
        /// Gets or sets host to which the underlying TCP connection is made.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the virtual host.
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// Gets or sets the port number to which the underlying TCP connection.
        /// </summary>
        public ushort Port { get; set; }

        ////public SslOption Ssl { get; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }
    }
}
