namespace HotterReload;

using System.Collections.Immutable;

/// <inheritdoc cref="IHotReloadHandler.OnHotReload(ImmutableArray{Type}, CancellationToken)"/>
public delegate ValueTask HotReloadHandler(ImmutableArray<Type> types, CancellationToken ct);