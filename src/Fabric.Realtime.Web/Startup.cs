namespace Fabric.Realtime.Web
{
    using System.Reflection;

    using Fabric.Realtime.Core;
    using Fabric.Realtime.Data.Stores;
    using Fabric.Realtime.Engine.Configuration;
    using Fabric.Realtime.Engine.Record;
    using Fabric.Realtime.Engine.Replay;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            this.Configuration = RealtimeConfiguration.BuildConfigurationRoot();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // Configure options
            services.AddOptions();

            services.AddSingleton<IBackgroundWorker, MessageReceiveWorker>();
            services.AddSingleton<IBackgroundWorker, MessageReplayWorker>();
            services.AddSingleton<IRealtimeConfiguration, RealtimeConfiguration>();

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

            app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });


            ////var context = app.ApplicationServices.GetRequiredService<RealtimeContext>();
            ////DbInitializer.Initialize(context);
        }
    }
}
