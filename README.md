# Baubit.Logging.DI

[![CircleCI](https://dl.circleci.com/status-badge/img/circleci/TpM4QUH8Djox7cjDaNpup5/2zTgJzKbD2m3nXCf5LKvqS/tree/master.svg?style=svg)](https://dl.circleci.com/status-badge/redirect/circleci/TpM4QUH8Djox7cjDaNpup5/2zTgJzKbD2m3nXCf5LKvqS/tree/master)
[![codecov](https://codecov.io/gh/pnagoorkar/Baubit.Logging.DI/branch/master/graph/badge.svg)](https://codecov.io/gh/pnagoorkar/Baubit.Logging.DI)<br/>
[![NuGet](https://img.shields.io/nuget/v/Baubit.Logging.DI.svg)](https://www.nuget.org/packages/Baubit.Logging.DI/)
![.NET Standard 2.0](https://img.shields.io/badge/.NET%20Standard-2.0-512BD4?logo=dotnet&logoColor=white)<br/>
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Known Vulnerabilities](https://snyk.io/test/github/pnagoorkar/Baubit.Logging.DI/badge.svg)](https://snyk.io/test/github/pnagoorkar/Baubit.Logging.DI)

## Overview

DI extension for Microsoft.Extensions.Logging. Enables registration of `ILoggerFactory` with configurable logging providers (Console, Debug) driven by configuration.

## Installation

```bash
dotnet add package Baubit.Logging.DI
```

Or via NuGet Package Manager:

```
Install-Package Baubit.Logging.DI
```

## Quick Start

Baubit.Logging.DI supports three patterns for module loading, consistent with [Baubit.DI](https://github.com/pnagoorkar/Baubit.DI):

### Pattern 1: Modules from appsettings.json

Load logging configuration from JSON. Module types and settings are defined in configuration files.

```csharp
await Host.CreateApplicationBuilder()
          .UseConfiguredServiceProviderFactory()
          .Build()
          .RunAsync();
```

**appsettings.json:**
```json
{
  "modules": [
    {
      "type": "Baubit.Logging.DI.Module, Baubit.Logging.DI",
      "configuration": {
        "addConsole": true,
        "addDebug": true
      }
    }
  ]
}
```

### Pattern 2: Modules from Code (IComponent)

Load logging programmatically using `IComponent`. No configuration file needed.

```csharp
public class AppComponent : AComponent
{
    protected override Result<ComponentBuilder> Build(ComponentBuilder builder)
    {
        return builder.WithModule<Module, Configuration>(config =>
                {
                    config.AddConsole = true;
                    config.AddDebug = true;
                });
    }
}

await Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings())
          .UseConfiguredServiceProviderFactory(componentsFactory: () => [new AppComponent()])
          .Build()
          .RunAsync();
```

### Pattern 3: Hybrid Loading (appsettings.json + IComponent)

Combine configuration-based and code-based module loading.

```csharp
await Host.CreateApplicationBuilder()
          .UseConfiguredServiceProviderFactory(componentsFactory: () => [new AppComponent()])
          .Build()
          .RunAsync();
```

### Using AddModule Directly

For direct service collection usage without Host:

```csharp
var services = new ServiceCollection();

services.AddModule<Module, Configuration>(config =>
{
    config.AddConsole = true;
    config.AddDebug = true;
});

var serviceProvider = services.BuildServiceProvider();
var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger<MyClass>();
logger.LogInformation("Hello, World!");
```

## Features

- **Console Logging**: Enable console output for log messages
- **Debug Logging**: Enable debug output for log messages
- **Configuration-Driven**: All providers can be enabled/disabled via configuration
- **Type-Safe Configuration**: Strongly-typed configuration with sensible defaults
- **.NET Standard 2.0**: Compatible with .NET Framework, .NET Core, and .NET 5+

## API Reference

### Configuration

Configuration class for the Logging DI module:

```csharp
public class Configuration : AConfiguration
{
    // Enable console logging provider. Defaults to false.
    public bool AddConsole { get; set; } = false;

    // Enable debug logging provider. Defaults to false.
    public bool AddDebug { get; set; } = false;
}
```

### Module

DI module for registering `ILoggerFactory`:

```csharp
public class Module : AModule<Configuration>
{
    public Module(IConfiguration configuration);
    public Module(Configuration configuration, List<IModule> nestedModules = null);
    public override void Load(IServiceCollection services);
}
```

### Usage Examples

**Console Logging Only:**

```csharp
services.AddModule<Module, Configuration>(config =>
{
    config.AddConsole = true;
});

var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger("MyApp");
logger.LogInformation("This will appear in the console");
```

**Debug Logging Only:**

```csharp
services.AddModule<Module, Configuration>(config =>
{
    config.AddDebug = true;
});

var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger("MyApp");
logger.LogDebug("This will appear in debug output");
```

**Both Console and Debug:**

```csharp
services.AddModule<Module, Configuration>(config =>
{
    config.AddConsole = true;
    config.AddDebug = true;
});
```

## Contributing

Contributions are welcome. Open an issue or submit a pull request.

## License

MIT License - see [LICENSE](LICENSE) file for details.