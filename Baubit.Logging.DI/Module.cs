using Baubit.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Baubit.Logging.DI
{
    /// <summary>
    /// DI module for registering <see cref="ILoggerFactory"/> with Microsoft.Extensions.DependencyInjection.
    /// Configures logging providers based on the <see cref="Configuration"/> settings.
    /// </summary>
    public class Module : Baubit.DI.Module<Configuration>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class from configuration.
        /// </summary>
        /// <param name="configuration">The configuration section to bind settings from.</param>
        public Module(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class with strongly-typed configuration.
        /// </summary>
        /// <param name="configuration">The strongly-typed configuration for this module.</param>
        /// <param name="nestedModules">Optional list of nested modules this module depends on.</param>
        public Module(Configuration configuration, List<IModule> nestedModules = null) : base(configuration, nestedModules)
        {
        }

        /// <summary>
        /// Registers logging services with the specified service collection.
        /// Adds logging providers based on the <see cref="Configuration"/> settings.
        /// </summary>
        /// <param name="services">The service collection to register services with.</param>
        public override void Load(IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                if (Configuration.AddConsole)
                {
                    builder.AddConsole();
                }

                if (Configuration.AddDebug)
                {
                    builder.AddDebug();
                }
            });

            base.Load(services);
        }
    }
}
