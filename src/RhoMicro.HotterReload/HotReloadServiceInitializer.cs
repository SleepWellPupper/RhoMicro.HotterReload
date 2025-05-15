namespace RhoMicro.HotterReload;

using Microsoft.Extensions.DependencyInjection;

internal sealed class HotReloadServicesInitializer(IServiceProvider ser) : IHotReloadServicesInitializer
{
    public void Initialize() => ser.GetRequiredService<IHotReloadService>();
}