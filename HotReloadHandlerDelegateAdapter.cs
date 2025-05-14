namespace HotterReload;
using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

internal sealed class HotReloadHandlerDelegateAdapter(HotReloadHandler implementation) : IHotReloadHandler
{
    public ValueTask OnHotReload(ImmutableArray<Type> types, CancellationToken ct) 
        => implementation.Invoke(types, ct);
}
