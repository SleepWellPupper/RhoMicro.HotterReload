
namespace HotterReload;

/// <summary>
/// Provides notifications about hot reload events.
/// </summary>
public interface IHotReloadService
{
    /// <summary>
    /// Invoked upon encountering a hot reload event.
    /// </summary>
    event EventHandler<HotReloadEventArgs>? OnHotReload;
}
