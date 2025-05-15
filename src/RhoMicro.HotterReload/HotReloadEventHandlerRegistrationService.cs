namespace RhoMicro.HotterReload;

using Microsoft.Extensions.Options;

internal sealed class HotReloadEventHandlerRegistrationService
{
    public HotReloadEventHandlerRegistrationService(IHotReloadService hotReloadService, IOptions<HotReloadEventHandlerRegistrationServiceOptions> options)
    {
        foreach(var handler in options.Value.EventHandlers)
            hotReloadService.OnHotReload += handler;
    }
}
