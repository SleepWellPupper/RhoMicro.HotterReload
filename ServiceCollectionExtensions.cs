namespace HotterReload;
using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Provides extension methods for adding hot reload capabilities to a service
/// collection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds hot reload capabilities to the service collection.
    /// </summary>
    /// <param name="services">
    /// The service collection to add hot reload capabilities to.
    /// </param>
    /// <returns>
    /// A builder object for adding hot reload handlers to the service collection.
    /// </returns>
    public static HotReloadBuilder AddHotReload(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services
            .AddActivatedSingleton<HotReloadEventHandlerRegistrationService>()
            .AddOptions<HotReloadEventHandlerRegistrationServiceOptions>()
            .Services
            .AddActivatedSingleton<HotReloadService>()
            .TryAddSingleton(typeof(IHotReloadService), sp => sp.GetRequiredService<HotReloadService>());

        var result = new HotReloadBuilder(services);

        return result;
    }
}
