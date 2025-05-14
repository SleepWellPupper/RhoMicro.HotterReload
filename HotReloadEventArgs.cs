
namespace HotterReload;

using System.Collections.Immutable;

/// <summary>
/// Provides event args for the hot reload service.
/// </summary>
public sealed class HotReloadEventArgs : EventArgs
{
    internal HotReloadEventArgs(ImmutableArray<Type> types, CancellationToken cancellationToken)
    {
        Types = types;
        CancellationToken = cancellationToken;
    }

    /// <summary>
    /// The types that were reloaded. If an empty array is passed, any type may
    /// have been reloaded.
    /// </summary>
    public ImmutableArray<Type> Types { get; }
    /// <summary>
    /// The cancellation token used to request handling to be cancelled. 
    /// Cancellation may be requested when the method is still executing when
    /// another hot reload is registered.
    /// </summary>
    public CancellationToken CancellationToken { get; }

    private readonly List<Task> _tasks = [];
    
    /// <summary>
    /// Adds a task representing handler work.
    /// </summary>
    /// <param name="task">
    /// The task representing handler work.
    /// </param>
    public void AddTask(Task task) => _tasks.Add(task);
    internal IReadOnlyList<Task> GetTasks() => _tasks;
}
