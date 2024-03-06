namespace Dal;

using DalApi;

sealed internal class DalList : IDal
{
	// safe-thread and lazy-initialization singleton: https://csharpindepth.com/articles/singleton
	private static readonly Lazy<DalList> lazy = new Lazy<DalList>(() => new DalList(), LazyThreadSafetyMode.ExecutionAndPublication);
	public static DalList Instance { get => lazy.Value; }

	private DalList() { }

	public ITask Task => new TaskImplementation();

	public IEngineer Engineer => new EngineerImplementation();

	public IDependency Dependency => new DependencyImplementation();

	/// <summary> Reset all lists to empty </summary>
	public void Reset()
	{
		DataSource.Engineers.Clear();
		DataSource.Tasks.Clear();
		DataSource.Dependencies.Clear();
		DataSource.Config.Reset();
	}

	public void SaveScheduledDate() { }

	public DateTime? GetStartProject() { return DateTime.Now; }

	/// <inheritdoc/>
	public void SaveClock(DateTime time) { }

	/// <inheritdoc/>
	public DateTime LoadClock() => DateTime.Now;
}
