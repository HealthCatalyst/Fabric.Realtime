using System;
using System.Threading;
using System.Threading.Tasks;
using Fabric.Realtime.Core;
using Microsoft.Extensions.Logging;

namespace Fabric.Realtime.Engine.Replay
{
    /// <summary>
    /// Listens for incoming messages and writes to a persistent store.
    /// </summary>
    public class MessageReplayWorker : IBackgroundWorker
    {
        private readonly ILogger logger;

        public MessageReplayWorker(ILogger<MessageReplayWorker> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task RunAsync(CancellationToken token)
        {
            try
            {
                await DoWork(token);
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Exiting background worker.");
            }
        }

        private async Task DoWork(CancellationToken token)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(5000, token);
                logger.LogInformation(string.Format("REPLAY: {0}", DateTime.Now.ToString("u")));
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
