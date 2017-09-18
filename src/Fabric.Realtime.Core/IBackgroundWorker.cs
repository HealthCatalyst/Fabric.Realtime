using System.Threading;
using System.Threading.Tasks;

namespace Fabric.Realtime.Core
{
    /// <summary>
    /// An asynchronous service.
    /// </summary>
    public interface IBackgroundWorker
    {
        /// <summary>
        /// The run async.
        /// </summary>
        /// <param name="token">
        /// The token used to signal cancellation.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task RunAsync(CancellationToken token);
    }
}