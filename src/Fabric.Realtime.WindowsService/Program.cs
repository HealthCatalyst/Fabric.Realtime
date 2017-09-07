namespace Fabric.Realtime.WindowsService
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;

    using Topshelf;

    /// <summary>
    /// Fabric Real-time Windows service.
    /// </summary>
    /// <remarks>
    /// Commands (see http://docs.topshelf-project.com/en/latest/overview/commandline.html):
    ///     install
    ///     uninstall
    ///     start
    ///     stop
    /// </remarks>
    public class Program
    {
        /// <summary>
        /// The main entry point for the Fabric Real-time Windows service.
        /// </summary>
        public static void Main()
        {
            HostFactory.Run(service =>
                {
                    service.SetServiceName(Properties.Resources.ServiceName);
                    service.SetDisplayName(ServiceDisplayName);
                    service.SetDescription(Properties.Resources.ServiceDescription);

                    service.Service<FabricServiceController>(
                        serviceConfigureation =>
                            {
                                serviceConfigureation.ConstructUsing(() => new FabricServiceController());
                                serviceConfigureation.WhenStarted((controller, hostControl) => controller.Start(hostControl));
                                serviceConfigureation.WhenStopped((controller, hostControl) => controller.Stop(hostControl));
                            });

                    service.StartAutomatically();
                    service.SetStartTimeout(TimeSpan.FromSeconds(10));
                    service.SetStopTimeout(TimeSpan.FromSeconds(10));
                    service.RunAsNetworkService();
                });
        }

        private static string ServiceDisplayName => string.Format(CultureInfo.CurrentCulture,
            Properties.Resources.ServiceDisplayName, Version);

        public static string Version
            => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

    }
}
