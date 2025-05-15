namespace RhoMicro.HotterReload;

/// <summary>
/// Initializes hot reload services.
/// Resolving and invoking this initializer is
/// not required when using a host that runs
/// hosted services.
/// </summary>
public interface IHotReloadServicesInitializer
{
    /// <summary>
    /// Initializes hot reload services.
    /// </summary>
    public void Initialize();
}