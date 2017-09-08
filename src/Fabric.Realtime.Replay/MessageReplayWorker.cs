namespace Fabric.Realtime.Replay
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Fabric.Realtime.Core;

    /// <summary>
    /// Listens for incoming messages and writes to a persistent store.
    /// </summary>
    public class MessageReplayWorker : IBackgroundWorker
    {
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

        private static async Task DoWork(CancellationToken token)
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(5000, token);
                Console.WriteLine(string.Format("REPLAY: {0}", DateTime.Now.ToString("u")));
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
