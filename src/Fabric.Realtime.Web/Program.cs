using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fabric.Realtime.Core;
using Microsoft.AspNetCore.Hosting;
using Fabric.Realtime.Engine.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fabric.Realtime.Web
{
    public class Program
    {
        private static readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        public static void Main(string[] args)
        {
            var configurationRoot = RealtimeConfiguration.BuildConfigurationRoot(args);
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseConfiguration(configurationRoot)
                //.UseApplicationInsights()
                .Build();

            Run(host);
            //host.Run();
        }

        private static void Run(IWebHost host)
        {
            var taskList = StartTasks(host);

            Console.WriteLine(@"Press <ctrl>+C to exit.");
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                tokenSource.Cancel();
            };
            Task.WaitAll(taskList);
        }

        public static Task[] StartTasks(IWebHost host)
        {
            var workers = host.Services.GetServices<IBackgroundWorker>();
            return workers.Select(worker => worker.RunAsync(tokenSource.Token)).ToArray();
        }

    }
}
