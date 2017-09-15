using System;
using System.Threading;
using System.Threading.Tasks;
using Fabric.Realtime.Core;
using Fabric.Realtime.Engine.Configuration;
using Microsoft.Extensions.Logging;

namespace Fabric.Realtime.Engine.Record
{
    /// <summary>
    /// Listens for incoming messages and replays messages from a persistent store.
    /// </summary>
    public class MessageReceiveWorker : IBackgroundWorker
    {
        private readonly ILogger logger;

        public MessageReceiveWorker(ILoggerFactory loggerFactory, IRealtimeConfiguration config)
        {
            logger = loggerFactory.CreateLogger(typeof(MessageReceiveWorker));
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
                await Task.Delay(1000, token);
                logger.LogInformation(string.Format("RECEIVE: {0}", DateTime.Now.ToString("u")));
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
