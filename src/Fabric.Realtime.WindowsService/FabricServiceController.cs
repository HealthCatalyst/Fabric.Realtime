using Fabric.Realtime.Engine.Configuration;
using Fabric.Realtime.Engine.Record;
using Fabric.Realtime.Engine.Replay;

namespace Fabric.Realtime.WindowsService
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Topshelf;

    /// <summary>
    /// The Windows service controller.
    /// </summary>
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Reviewed. Suppression is OK here.")]
    public class FabricServiceController : ServiceControl
    {
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        private Task[] workerTasks;

        /// <summary>
        /// Start the Windows service. 
        /// This method may start-up multiple worker tasks.
        /// </summary>
        /// <param name="hostControl">
        /// The Topshelf host control instance
        /// </param>
        /// <returns>
        /// true if start-up was successful; otherwise false.
        /// </returns>
        [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
            "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        public bool Start(HostControl hostControl)
        {
            Console.WriteLine(@"FabricServiceController Starting...");

            hostControl.RequestAdditionalTime(TimeSpan.FromSeconds(10));

            this.workerTasks = StartTasks();

            Console.WriteLine(@"FabricServiceController Started");

            return true;
        }

        /// <summary>
        /// Stop the service.
        /// </summary>
        /// <param name="hostControl">
        /// The Topshelf host control instance
        /// </param>
        /// <returns>
        /// true if shut-down was successful; otherwise false.
        /// </returns>
        [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
            "SA1650:ElementDocumentationMustBeSpelledCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        public bool Stop(HostControl hostControl)
        {
            Console.WriteLine(@"FabricServiceController stopping");
            try
            {
                this.tokenSource.Cancel();
                Task.WaitAll(this.workerTasks, 1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return true;
        }

        public Task[] StartTasks()
        {
            return new[]
            {
                new MessageReceiveWorker(new RealtimeConfiguration(RealtimeConfiguration.BuildConfigurationRoot(new string[]{}))).RunAsync(tokenSource.Token);
                new MessageReplayWorker().RunAsync(tokenSource.Token)
            };
        }

    }
}
