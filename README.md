# RhoMicro.HotterReload

Integrates dotnet hot reload events into service collections.

## Installation

```xml
<ItemGroup>
    <PackageReference Update="RhoMicro.HotterReload" Version="1.0.0" />
</ItemGroup>
```

## Use

```cs
builder.Services
    .AddHotReload()
    .AddHandler((_, _) => Console.WriteLine("Hot Reload Detected"));
```

## Use

Add hot reload services to a service collection:
```cs
var hotReloadBuilder = services.AddHotReload();
```

On the returned builder, use the various overloads of `AddHandler` to register handlers for hot reload events:

In order to initialize the hot relead integration, resolve the `IHotReloadServiceInitializer` and invoke its `Initialize` method:
```cs
serviceProvider.GetRequiredService<IHotReloadServiceInitializer>().Initialize();
```

When added to a service collection on a host builder such as the `WebApplicationBuilder`, this step is not necessary.
