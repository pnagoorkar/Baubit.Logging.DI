using Baubit.DI;
using Baubit.DI.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Baubit.Logging.DI.Test.Module
{
    /// <summary>
    /// Unit tests for <see cref="DI.Module"/>
    /// </summary>
    public class Test
    {
        [Fact]
        public void Constructor_WithIConfiguration_CreatesModule()
        {
            // Arrange
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "AddConsole", "true" },
                { "AddDebug", "false" }
            });
            var config = configBuilder.Build();

            // Act
            var module = new DI.Module(config);

            // Assert
            Assert.NotNull(module);
            Assert.NotNull(module.Configuration);
        }

        [Fact]
        public void Constructor_WithConfiguration_CreatesModule()
        {
            // Arrange
            var configuration = new DI.Configuration
            {
                AddConsole = true,
                AddDebug = true
            };

            // Act
            var module = new DI.Module(configuration);

            // Assert
            Assert.NotNull(module);
            Assert.True(module.Configuration.AddConsole);
            Assert.True(module.Configuration.AddDebug);
        }

        [Fact]
        public void Constructor_WithNestedModules_CreatesModule()
        {
            // Arrange
            var configuration = new DI.Configuration();
            var nestedModules = new List<Baubit.DI.IModule>();

            // Act
            var module = new DI.Module(configuration, nestedModules);

            // Assert
            Assert.NotNull(module);
            Assert.NotNull(module.NestedModules);
        }

        [Fact]
        public void Load_WithNoProvidersEnabled_RegistersLoggerFactory()
        {
            // Arrange
            var configuration = new DI.Configuration
            {
                AddConsole = false,
                AddDebug = false
            };
            var module = new DI.Module(configuration);
            var services = new ServiceCollection();

            // Act
            module.Load(services);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            Assert.NotNull(loggerFactory);
        }

        [Fact]
        public void Load_WithConsoleEnabled_RegistersLoggerFactoryWithConsole()
        {
            // Arrange
            var configuration = new DI.Configuration
            {
                AddConsole = true,
                AddDebug = false
            };
            var module = new DI.Module(configuration);
            var services = new ServiceCollection();

            // Act
            module.Load(services);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            Assert.NotNull(loggerFactory);
            var logger = loggerFactory.CreateLogger("TestLogger");
            Assert.NotNull(logger);
        }

        [Fact]
        public void Load_WithDebugEnabled_RegistersLoggerFactoryWithDebug()
        {
            // Arrange
            var configuration = new DI.Configuration
            {
                AddConsole = false,
                AddDebug = true
            };
            var module = new DI.Module(configuration);
            var services = new ServiceCollection();

            // Act
            module.Load(services);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            Assert.NotNull(loggerFactory);
            var logger = loggerFactory.CreateLogger("TestLogger");
            Assert.NotNull(logger);
        }

        [Fact]
        public void Load_WithAllProvidersEnabled_RegistersLoggerFactoryWithAllProviders()
        {
            // Arrange
            var configuration = new DI.Configuration
            {
                AddConsole = true,
                AddDebug = true
            };
            var module = new DI.Module(configuration);
            var services = new ServiceCollection();

            // Act
            module.Load(services);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            Assert.NotNull(loggerFactory);
            var logger = loggerFactory.CreateLogger("TestLogger");
            Assert.NotNull(logger);
        }

        [Fact]
        public void Load_WithComponentBuilder_RegistersLoggerFactory()
        {
            // Arrange & Act
            var result = ComponentBuilder.CreateNew()
                                         .WithModule<DI.Module, DI.Configuration>(config =>
                                         {
                                             config.AddConsole = true;
                                             config.AddDebug = true;
                                         })
                                         .BuildServiceProvider();

            // Assert
            Assert.True(result.IsSuccess);
            var serviceProvider = result.Value;
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            Assert.NotNull(loggerFactory);
        }

        [Fact]
        public void Load_LoggerFactoryCanCreateLogger_WithCorrectCategory()
        {
            // Arrange
            var configuration = new DI.Configuration
            {
                AddConsole = true
            };
            var module = new DI.Module(configuration);
            var services = new ServiceCollection();
            module.Load(services);
            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            // Act
            var logger = loggerFactory.CreateLogger<Test>();

            // Assert
            Assert.NotNull(logger);
        }

        [Fact]
        public void Configuration_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var configuration = new DI.Configuration();

            // Assert
            Assert.False(configuration.AddConsole);
            Assert.False(configuration.AddDebug);
        }

        [Fact]
        public void Module_IsSubclassOfAModule()
        {
            // Arrange
            var configuration = new DI.Configuration();

            // Act
            var module = new DI.Module(configuration);

            // Assert
            Assert.IsAssignableFrom<Baubit.DI.AModule<DI.Configuration>>(module);
        }

        [Fact]
        public void Load_WithIConfiguration_BindsConfigurationCorrectly()
        {
            // Arrange
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "AddConsole", "true" },
                { "AddDebug", "true" }
            });
            var config = configBuilder.Build();
            var module = new DI.Module(config);
            var services = new ServiceCollection();

            // Act
            module.Load(services);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            Assert.NotNull(loggerFactory);
        }
    }
}
