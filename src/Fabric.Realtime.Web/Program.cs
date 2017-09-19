namespace Fabric.Realtime.Web
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Fabric.Realtime.Core;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public class Program
    {
        private static readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        private static Task[] workerTasks;

        private static void StartupWorkers(IWebHost host)
        {
            workerTasks = StartTasks(host);

            Console.WriteLine(@"Press <ctrl>+C to exit.");
            Console.CancelKeyPress += (sender, eventArgs) =>
                {
                    tokenSource.Cancel();
                };
        }

        public static Task[] StartTasks(IWebHost host)
        {
            var workers = host.Services.GetServices<IBackgroundWorker>();
            return workers.Select(worker => worker.RunAsync(tokenSource.Token)).ToArray();
        }

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            StartupWorkers(host);
            host.Run();
        }
    }
}
