namespace DO;

public record Dependency
(
	int Id,
	int DependentTask,
	int DependsOnTask
)
{
	public Dependency(int id = 0) : this(id, 0, 0) { }
}