using System.Linq;
using Catalyst.Logging.Abstractions;
using Fabric.Realtime.Data.Stores;
using Microsoft.EntityFrameworkCore;

namespace Fabric.Realtime.ConsoleApp
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Fabric.Realtime.Core;
    using DryIoc;

    /// <summary>
    /// Fabric Real-time application (.NET Core) that allows you 
    /// to run from the command line or via Docker.
    /// </summary>
    public class Program
    {
        private static readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
        private static IContainer container;
        private static ILoggingService log;

        /// <summary>
        /// The main entry point for the Fabric Real-time application.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            try
            {
                Initialize();

                log.Info(@"FabricServiceController Starting...");
                Run();
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("TaskCanceledException");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void Initialize()
        {
            container = Bootstrapper.Initialize();
            log = container.Resolve<ILoggingService>();

            var optionsBuilder = new DbContextOptionsBuilder<RealtimeContext>();
            optionsBuilder.UseSqlServer(
                "Server=(local);Database=FabricRealtime;Trusted_Connection=True;MultipleActiveResultSets=true");
            using (var dbContext = new RealtimeContext(optionsBuilder.Options))
            {
                log.Info(@"Creating FabricRealtime database...");
                DbInitializer.Initialize(dbContext);
                log.Info(@"Finished FabricRealtime database creation.");
            }
        }

        private static void Run()
        {
            var taskList = StartTasks();

            Console.WriteLine(@"Press <ctrl>+C to exit.");
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                log.Info(@"Cancelling workers...");
                tokenSource.Cancel();
            };
            Task.WaitAll(taskList);
        }

        public static Task[] StartTasks()
        {
            var workers = container.ResolveMany<IBackgroundWorker>();
            return workers.Select(worker => worker.RunAsync(tokenSource.Token)).ToArray();
        }
    }
}
