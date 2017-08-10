namespace Fabric.Realtime
{
    using Fabric.Realtime.EventBus.Implementations;
    using Fabric.Realtime.EventBus.Models;
    using Fabric.Realtime.Handlers;
    using Fabric.Realtime.Transformers;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true).AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddTransient<IInterfaceEngineEventHandler, InterfaceEngineEventHandler>();

            services.AddSingleton<IInterfaceEngineMessageTransformer>(new InterfaceEngineMessageTransformer());

            services.AddSingleton<IQueueService>(
                new RabbitMQQueueService(
                    primaryHost: "192.168.137.2",
                    secondaryHost: string.Empty,
                    messageExchange: "fabric.interfaceengine",
                    messageQueue: "mirth.connect",
                    routingKey: "mirth.connect.inbound",
                    eventHandler: services.BuildServiceProvider().GetRequiredService<IInterfaceEngineEventHandler>()));
        }
    }
}