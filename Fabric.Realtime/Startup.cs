namespace Fabric.Realtime
{
    using System.Reflection;

    using Fabric.Realtime.Domain.Stores;
    using Fabric.Realtime.EventBus.Implementations;
    using Fabric.Realtime.EventBus.Models;
    using Fabric.Realtime.Handlers;
    using Fabric.Realtime.Transformers;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;

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



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddDbContext<RealtimeContext>(
                options =>
                    {
                        options.UseSqlServer(
                            this.Configuration["ConnectionString"],
                            sqlServerOptionsAction: sqlOptions =>
                                {
                                    sqlOptions.MigrationsAssembly(
                                        typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                    sqlOptions.EnableRetryOnFailure(
                                        maxRetryCount: 5,
                                        maxRetryDelay: System.TimeSpan.FromSeconds(30),
                                        errorNumbersToAdd: null);
                                });
                    });

            services.AddTransient<IInterfaceEngineEventHandler, InterfaceEngineEventHandler>();

            services.AddSingleton<IInterfaceEngineMessageTransformer>(new InterfaceEngineMessageTransformer());

            services.AddSingleton<IQueueService>(
                new RabbitMQQueueService(
                    hostName: "rabbitmq",
                    port: int.Parse(this.Configuration["MessageBrokerPort"]),
                    secondaryHost: string.Empty,
                    messageExchange: "fabric.interfaceengine",
                    messageQueue: "mirth.connect",
                    routingKey: "mirth.connect.inbound",
                    eventHandler: services.BuildServiceProvider().GetRequiredService<IInterfaceEngineEventHandler>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            var services = app.ApplicationServices;
            try
            {
                var context = services.GetRequiredService<RealtimeContext>();
                DbInitializer.Initialize(context);
            }
            catch (Exception ex)
            {
                // log error
            }
        }
    }
}