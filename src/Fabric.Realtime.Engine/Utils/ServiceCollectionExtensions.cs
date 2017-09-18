using Fabric.Realtime.Core.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fabric.Realtime.Engine.Utils
{
    public static class ServiceCollectionExtensions
    {
        public static TConfig ConfigurePoco<TConfig>(this IServiceCollection services, IConfiguration configuration)
            where TConfig : class, new()
        {
            Guard.ArgumentNotNull(services, nameof(services));
            Guard.ArgumentNotNull(configuration, nameof(configuration));

            var config = new TConfig();
            configuration.Bind(config);
            services.AddSingleton(config);
            return config;
        }
    }
}
