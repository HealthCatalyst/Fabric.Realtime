namespace Fabric.Realtime.ConsoleApp
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using DryIoc;
    using Fabric.Realtime.Core;
    using Fabric.Realtime.Record;
    using Fabric.Realtime.Replay;
    
    /// <summary>
    /// Fabric Real-time application (.NET Core) that allows you 
    /// to run from the command line or via Docker.
    /// </summary>
    public class Program
    {
        private static readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
        private static readonly Container container = new Container();

        /// <summary>
        /// The main entry point for the Fabric Real-time application.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Console.WriteLine(@"FabricServiceController Starting...");

            try
            {
                Initialize();
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
            container.Register<IBackgroundWorker, MessageReceiveWorker>();
            container.Register<IBackgroundWorker, MessageReplayWorker>();

        }

        private static void Run()
        {
            var taskList = StartTasks();

            Console.WriteLine(@"Press <ctrl>+C to exit.");
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Console.WriteLine(@"Cancelling workers...");
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
