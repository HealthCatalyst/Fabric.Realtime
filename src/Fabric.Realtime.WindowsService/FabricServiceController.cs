namespace Fabric.Realtime.WindowsService
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;

    using Fabric.Realtime.Record;

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

        private Task workerTask;

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

            var worker = new MessageReceiveWorker();
            this.workerTask = Task.Factory.StartNew(() => worker.RunAsync(this.tokenSource.Token));

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
                this.workerTask.Wait(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return true;
        }
    }
}
