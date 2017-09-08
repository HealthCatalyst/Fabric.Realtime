namespace Fabric.Realtime.Record
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Fabric.Realtime.Core;

    /// <summary>
    /// Listens for incoming messages and replays messages from a persistent store.
    /// </summary>
    public class MessageReceiveWorker : IBackgroundWorker
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
                await Task.Delay(1000, token);
                Console.WriteLine(string.Format("RECEIVE: {0}", DateTime.Now.ToString("u")));
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
