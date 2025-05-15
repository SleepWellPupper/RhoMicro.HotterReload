namespace RhoMicro.HotterReload;

using System.Collections.Immutable;

/// <summary>
/// Implements functionality to react to a hot reload event.
/// </summary>
public interface IHotReloadHandler
{
    /// <summary>
    /// Handles a hot reload event.
    /// </summary>
    /// <param name="ct">
    /// The cancellation token used to request handling to be cancelled. 
    /// Cancellation may be requested when the method is still executing when
    /// another hot reload is registered.
    /// </param>
    /// <param name="types">
    /// The types that were reloaded. If an empty array is passed, any type may
    /// have been reloaded.
    /// </param>
    /// <returns>
    /// A value task representing the handling operation.
    /// </returns>
    ValueTask OnHotReload(ImmutableArray<Type> types, CancellationToken ct);
}
