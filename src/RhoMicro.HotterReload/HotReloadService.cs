namespace RhoMicro.HotterReload;

using System.Collections.Immutable;

using Microsoft.Extensions.Logging;

internal sealed class HotReloadService : IHotReloadService, IDisposable
{
    public HotReloadService(IEnumerable<IHotReloadHandler> handlers, ILogger<HotReloadService> logger)
    {
        _handlers = [.. handlers];
        _logger = logger;
        MetadataUpdateHandler.Register(this);
    }

    private readonly ImmutableArray<IHotReloadHandler> _handlers;
    private readonly ILogger<HotReloadService> _logger;

    public event EventHandler<HotReloadEventArgs>? OnHotReload;

    public async void NotifyHotReload(ImmutableArray<Type> types, CancellationToken ct)
    {
        try
        {
            var handlerTask = NotifyHotReloadCore(types, ct);

            if(!handlerTask.IsCompletedSuccessfully)
                await handlerTask;
        } catch(OperationCanceledException)
            when(ct.IsCancellationRequested)
        {
            _logger.LogDebug("Cancelled hot reload handling.");
        } catch(Exception ex)
        {
            _logger.LogError(ex, "Error while handling hot reload.");
        }
    }

    private async ValueTask NotifyHotReloadCore(ImmutableArray<Type> types, CancellationToken ct)
    {
        _logger.LogDebug("Handling hot reload for types: {Types}", String.Join(", ", types.Select(t => t.Name)));

        ct.ThrowIfCancellationRequested();

        var handlersTask = ExecuteHandlers(types, ct);
        if(!handlersTask.IsCompletedSuccessfully)
            await handlersTask;

        var eventsTask = ExecuteEvent(types, ct);
        if(!eventsTask.IsCompletedSuccessfully)
            await eventsTask;

        _logger.LogDebug("Done handling hot reload.");
    }

    private async ValueTask ExecuteEvent(ImmutableArray<Type> types, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        _logger.LogDebug("Invoking event.");

        var eventArgs = new HotReloadEventArgs(types, ct);

        OnHotReload?.Invoke(this, eventArgs);

        var tasks = eventArgs.GetTasks();
        if(tasks.Count > 0)
            await Task.WhenAll(tasks);

        _logger.LogDebug("Done invoking event.");
    }

    private async ValueTask ExecuteHandlers(ImmutableArray<Type> types, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        _logger.LogDebug("Invoking handlers.");

        foreach(var handler in _handlers)
        {
            ct.ThrowIfCancellationRequested();

            var handlerTask = ExecuteHandler(handler, types, ct);

            if(!handlerTask.IsCompletedSuccessfully)
                await handlerTask;
        }

        _logger.LogDebug("Done invoking handlers.");
    }

    private async ValueTask ExecuteHandler(IHotReloadHandler handler, ImmutableArray<Type> types, CancellationToken ct)
    {
        _logger.LogDebug("Executing hot reload handler '{Handler}'.", handler);

        ct.ThrowIfCancellationRequested();

        try
        {
            var handlerTask = handler.OnHotReload(types, ct);

            if(!handlerTask.IsCompletedSuccessfully)
                await handlerTask;
        } catch(Exception ex)
            when(ex is not OperationCanceledException || !ct.IsCancellationRequested)
        {
            _logger.LogError(ex, "Error while executing hot reload handler '{Handler}'.", handler);
        }

        _logger.LogDebug("Done executing hot reload handler '{Handler}'.", handler);
    }

    private Boolean _disposedValue;

    private void DisposeCore()
    {
        if(!_disposedValue)
        {
            MetadataUpdateHandler.Remove(this);
            _disposedValue = true;
        }
    }

    ~HotReloadService() => DisposeCore();

    public void Dispose()
    {
        DisposeCore();
        GC.SuppressFinalize(this);
    }
}