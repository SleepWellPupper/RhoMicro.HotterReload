namespace RhoMicro.HotterReload;
using System;
using System.Collections.Concurrent;

internal sealed class HotReloadEventHandlerRegistrationServiceOptions
{
    public ConcurrentBag<EventHandler<HotReloadEventArgs>> EventHandlers { get; } = [];
}
