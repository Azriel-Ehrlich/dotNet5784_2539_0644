namespace DalApi;

public interface IDal
{
	ITask Task { get; }
	IEngineer Engineer { get; }
	IDependency Dependency { get; }

	/// <summary> Reset the entire database </summary>
	abstract void Reset();
}
