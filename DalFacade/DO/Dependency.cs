namespace DO;

/// <summary>
/// A dependency between two tasks (which can start only after the other is done)
/// </summary>
/// <param name="Id">Dependency's running id</param>
/// <param name="DependentTask">Id of dependent task</param>
/// <param name="DependsOnTask">Id of task that must be done first</param>
public record Dependency
(
	int Id,
	int? DependentTask = null,
	int? DependsOnTask = null
)
{
	public Dependency(int id = 0) : this(id, 0, 0) { }
	public Dependency(int DependentTask, int DependsOnTask) : this(0, DependentTask, DependsOnTask) { }
}