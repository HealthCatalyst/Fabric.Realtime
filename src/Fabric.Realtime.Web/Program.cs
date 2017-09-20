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

    using Serilog;
    using Serilog.Events;

    /// <summary>
    /// The Fabric.Realtime web application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The token source used to signal worker tasks to exit.
        /// </summary>
        private static readonly CancellationTokenSource TokenSource = new CancellationTokenSource();

        /// <summary>
        /// The worker tasks.
        /// </summary>
        private static Task[] workerTasks;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        public static void Main(string[] args)
        {
            InitializeLogging();
            Log.Logger.Information(@"Starting application.");

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseApplicationInsights()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();

            StartupWorkers(host.Services);
            host.Run();
        }

        /// <summary>
        /// The startup workers.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider.
        /// </param>
        private static void StartupWorkers(IServiceProvider serviceProvider)
        {
            workerTasks = StartTasks(serviceProvider);

            Console.WriteLine(@"Press <ctrl>+C to exit.");
            Console.CancelKeyPress += (sender, eventArgs) =>
                {
                    TokenSource.Cancel();
                };
        }

        /// <summary>
        /// Start background worker tasks.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider.
        /// </param>
        /// <returns>
        /// An array of tasks.
        /// </returns>
        private static Task[] StartTasks(IServiceProvider serviceProvider)
        {
            var workers = serviceProvider.GetServices<IBackgroundWorker>();
            return workers.Select(worker => worker.RunAsync(TokenSource.Token)).ToArray();
        }

        /// <summary>
        /// Configure and initialize the logging system.
        /// </summary>
        private static void InitializeLogging()
        {
            // Using Serilog
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.RollingFile("logs/Realtime-{Date}.log")
                .CreateLogger();
        }
    }
}
