namespace Fabric.Realtime.Engine.Record
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Fabric.Realtime.Core;
    using Fabric.Realtime.Core.Utils;
    using Fabric.Realtime.Engine.Configuration;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Listens for incoming messages and replays messages from a persistent store.
    /// </summary>
    public class MessageReceiveWorker : IBackgroundWorker
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageReceiveWorker"/> class.
        /// </summary>
        /// <param name="logger">
        /// The logger.
        /// </param>
        /// <param name="config">
        /// The configuration.
        /// </param>
        public MessageReceiveWorker(ILogger<MessageReceiveWorker> logger, IRealtimeConfiguration config)
        {
            Guard.ArgumentNotNull(logger, nameof(logger));
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task RunAsync(CancellationToken token)
        {
            try
            {
                await this.DoWork(token);
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Exiting background worker.");
            }
        }

        /// <summary>
        /// Asynchronous worker.
        /// </summary>
        /// <param name="token">
        /// The cancellation token to signal the worker to exit.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task DoWork(CancellationToken token)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(1000, token);
                this.logger.LogInformation(string.Format("RECEIVE: {0}", DateTime.Now.ToString("u")));
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
