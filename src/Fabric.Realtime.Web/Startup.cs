using Fabric.Realtime.Core;
using Fabric.Realtime.Engine.Configuration;
using Fabric.Realtime.Engine.Record;
using Fabric.Realtime.Engine.Replay;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fabric.Realtime.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IBackgroundWorker, MessageReceiveWorker>();
            services.AddSingleton<IBackgroundWorker, MessageReplayWorker>();
            services.AddSingleton<IRealtimeConfiguration, RealtimeConfiguration>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //Factory.AddConsole(LogLevel.Trace, includeScopes: true);
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            //loggerFactory.AddFile("logs/Realtime-{Date}.json",
            //    isJson: false,
            //    minimumLevel: LogLevel.Information);

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }

    }
}
