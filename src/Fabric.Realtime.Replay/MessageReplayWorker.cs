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
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(1000, token);
                    Console.WriteLine(DateTime.Now.ToString("u"));
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Exiting background worker.");
            }
        }
    }
}
