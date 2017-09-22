namespace Fabric.Realtime.Web
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    using Fabric.Realtime.Core;
    using Fabric.Realtime.Data.Stores;
    using Fabric.Realtime.Engine.Configuration;
    using Fabric.Realtime.Engine.Handlers;
    using Fabric.Realtime.Engine.Record;
    using Fabric.Realtime.Engine.Transformers;
    using Fabric.Realtime.Services;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Serilog;

    using Swashbuckle.AspNetCore.Swagger;

    /// <summary>
    /// Application startup and initialization.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">
        /// The web hosting environment.
        /// </param>
        public Startup(IHostingEnvironment env)
        {
            this.Configuration = RealtimeConfiguration.BuildConfigurationRoot();
        }

        /// <summary>
        /// Gets the the configuration root.
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // Configure options
            services.AddOptions();

            // Logging
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            // Register application services
            services.AddSingleton<IRealtimeConfiguration>(new RealtimeConfiguration(this.Configuration));
            services.AddSingleton<IBackgroundWorker, MessageReceiveWorker>();
            services.AddTransient<IMessageStoreService, MessageStoreService>();
            services.AddTransient<IRealtimeSubscriptionService, RealtimeSubscriptionService>();

            var databaseSettings = this.Configuration.GetSection("Database").Get<DatabaseSettings>();
            services.AddDbContext<RealtimeContext>(
                options =>
                {
                    options.UseSqlServer(
                        databaseSettings.ConnectionString,
                        sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                            sqlOptions.EnableRetryOnFailure();
                        });
                });

            // Interface engine services
            services.AddSingleton<IInterfaceEngineEventHandler, InterfaceEngineEventHandler>();
            services.AddSingleton<IInterfaceEngineMessageTransformer>(new InterfaceEngineMessageTransformer());

            // Use Swashbuckle for documenting APIs built on ASP.NET Core
            services.AddSwaggerGen(
                c =>
                    {
                        c.SwaggerDoc("v1", new Info { Title = "Fabric.Realtime API", Version = "v1" });
                        c.DescribeAllEnumsAsStrings();
                    });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">
        /// The app.
        /// </param>
        /// <param name="env">
        /// The env.
        /// </param>
        /// <param name="loggerFactory">
        /// The logger factory.
        /// </param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(
                routes => { routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}"); });

            // Insert middleware to expose the generated Swagger as JSON endpoint.
            app.UseSwagger();

            // Insert the swagger-ui middleware to expose interactive documentation.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fabric.Realtime API V1"); });

            // Create database if it does not exist. 
            var context = app.ApplicationServices.GetRequiredService<RealtimeContext>();
            context.Database.EnsureCreated();
        }
    }
}
