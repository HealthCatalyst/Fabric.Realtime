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

    using Fabric.Realtime.EventBus.Services;

    using Swashbuckle.AspNetCore.Swagger;

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

            services.AddSingleton<MessageTypeSubscriberService, MessageTypeSubscriberService>();

            services.AddSingleton<ExternalApplicationQueueService>(new ExternalApplicationQueueService(
                new MessageBrokerExchange(hostName: this.Configuration["MessageBrokerHostName"],
                    port: int.Parse(this.Configuration["MessageBrokerPort"]),
                    exchange: this.Configuration["ExternalApplicationExchange"]))
                );

            services.AddSingleton<IInterfaceEngineEventHandler, InterfaceEngineEventHandler>();

            services.AddSingleton<IInterfaceEngineMessageTransformer>(new InterfaceEngineMessageTransformer());

            services.AddSingleton<MessageBrokerExchangeClient>(
                new MessageBrokerExchangeClient(
                    hostName: this.Configuration["MessageBrokerHostName"],
                    port: int.Parse(this.Configuration["MessageBrokerPort"]),
                    exchange: this.Configuration["InterfaceEngineExchange"],
                    queue: this.Configuration["InterfaceEngineQueue"],
                    routingKey: this.Configuration["InterfaceEngineRoutingKey"]));

            services.AddSingleton<InterfaceEngineQueueService, InterfaceEngineQueueService>();

            services.AddSwaggerGen(
                c =>
                    {
                        c.SwaggerDoc("v1", new Info() { Title = "Fabric.Realtime API", Version = "v1" }); 
                        c.DescribeAllEnumsAsStrings();
                    });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
            app.UseSwagger().UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fabric.Realtime API V1"); });

            var services = app.ApplicationServices;
            try
            {
                var context = services.GetRequiredService<RealtimeContext>();
                DbInitializer.Initialize(context);

                MessageTypeSubscriberService messageEventSubscriberService = services.GetRequiredService<MessageTypeSubscriberService>();
                messageEventSubscriberService.Initialize();

                var interfaceEngineQueueService = services.GetRequiredService<InterfaceEngineQueueService>();
                interfaceEngineQueueService.Initialize();

                var externalApplicationQueueService = services.GetRequiredService<ExternalApplicationQueueService>();
                externalApplicationQueueService.Initialize();
            }
            finally
            {
            }
        }
    }
}