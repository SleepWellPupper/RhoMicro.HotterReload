using System.Reflection.Metadata;

using HotterReload;

[assembly: MetadataUpdateHandler(typeof(MetadataUpdateHandler))]

namespace HotterReload;

using System.Collections.Immutable;
using System.Diagnostics;

internal static class MetadataUpdateHandler
{
    private static readonly HashSet<HotReloadService> _services = [];

#if NET9_0_OR_GREATER
    private static readonly Lock _lock = new();
#else
    private static readonly Object _lock = new();
#endif

    private static CancellationTokenSource _previousCts = new();

    public static void Register(HotReloadService service)
    {
        lock(_lock)
        {
            var result = _services.Add(service);
            Debug.Assert(result is true);
        }
    }

    public static void Remove(HotReloadService service)
    {
        lock(_lock)
        {
            var result = _services.Remove(service);
            Debug.Assert(result is true);
        }
    }

    public static void UpdateApplication(Type[]? updatedTypes)
    {
        var newCts = new CancellationTokenSource();

        Interlocked.Exchange(ref _previousCts, newCts).Cancel();

        var types = ImmutableArray.Create(updatedTypes ?? []);
        var ct = newCts.Token;

        lock(_lock)
        {
            foreach(var service in _services)
            {
                if(ct.IsCancellationRequested)
                    return;

                service.NotifyHotReload(types, ct);
            }
        }
    }
}
