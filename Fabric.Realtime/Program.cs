namespace Fabric.Realtime
{
    using System.IO;

    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder().UseKestrel().UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>().UseApplicationInsights().Build();

            host.Run();
        }
    }
}