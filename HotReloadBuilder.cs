namespace HotterReload;
using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

/// <summary>
/// Provides methods for adding hot reload handlers to a service collection.
/// </summary>
/// <param name="services">
/// The service collection to add handlers to.
/// </param>
public sealed class HotReloadBuilder(IServiceCollection services)
{
    /// <summary>
    /// Gets the underlying service collection.
    /// </summary>
    public IServiceCollection Services => services;
    /// <summary>
    /// Registers a handler type to the service collection.
    /// </summary>
    /// <typeparam name="THandler">
    /// The type of handler to register.
    /// </typeparam>
    /// <param name="lifetime">
    /// The lifetime of the registered handler.
    /// </param>
    /// <returns>
    /// A reference to this instance, for chaining of further method calls.
    /// </returns>
    public HotReloadBuilder AddHandler<THandler>(ServiceLifetime lifetime = ServiceLifetime.Singleton)
        where THandler : IHotReloadHandler
    {
        services.TryAddEnumerable(
            ServiceDescriptor.Describe(
                typeof(IHotReloadHandler),
                typeof(THandler),
                lifetime));

        return this;
    }
    /// <summary>
    /// Registers a handler factory to the service collection.
    /// </summary>
    /// <param name="factory">
    /// The factory to register.
    /// </param>
    /// <param name="lifetime">
    /// The lifetime of the registered handler.
    /// </param>
    /// <returns>
    /// A reference to this instance, for chaining of further method calls.
    /// </returns>
    public HotReloadBuilder AddHandler(Func<IServiceProvider, IHotReloadHandler> factory, ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        services.TryAddEnumerable(
            ServiceDescriptor.Describe(
                typeof(IHotReloadHandler),
                factory,
                lifetime));

        return this;
    }
    /// <summary>
    /// Registers a handler instance to the service collection.
    /// </summary>
    /// <param name="instance">
    /// The handler instance to register.
    /// </param>
    /// <returns>
    /// A reference to this instance, for chaining of further method calls.
    /// </returns>
    public HotReloadBuilder AddHandler(IHotReloadHandler instance)
    {
        services.TryAddEnumerable(ServiceDescriptor.Singleton(instance));

        return this;
    }
    /// <summary>
    /// Registers a handler to the service collection.
    /// </summary>
    /// <param name="handler">
    /// The handler to register.
    /// </param>
    /// <returns>
    /// A reference to this instance, for chaining of further method calls.
    /// </returns>
    public HotReloadBuilder AddHandler(HotReloadHandler handler) => AddHandler(new HotReloadHandlerDelegateAdapter(handler));
    /// <summary>
    /// Registers a handler to the service collection.
    /// </summary>
    /// <param name="handler">
    /// The handler to register.
    /// </param>
    /// <returns>
    /// A reference to this instance, for chaining of further method calls.
    /// </returns>
    public HotReloadBuilder AddHandler(EventHandler<HotReloadEventArgs> handler)
    {
        _ = services.Configure<HotReloadEventHandlerRegistrationServiceOptions>(o => o.EventHandlers.Add(handler));

        return this;
    }
}
