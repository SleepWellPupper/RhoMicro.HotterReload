using ExploratoryTests;

using HotterReload;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.AddHotReload().AddHandler((_, _) => Console.WriteLine("Hot Reload Detected"));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();