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

	private DateTime ScheduledStartDate = DateTime.MinValue;

	/// <summary> Reset all lists to empty </summary>
	public void Reset()
	{
		DataSource.Engineers.Clear();
		DataSource.Tasks.Clear();
		DataSource.Dependencies.Clear();
		DataSource.Config.Reset();
	}

	public void SaveScheduledDate()
	{
		// get the earliest start date
		IEnumerable<DO.Task?> tasks = Task.ReadAll().Where(t => t is null || t.ScheduledDate is not null);

		ScheduledStartDate = DateTime.MinValue;
		if (tasks.Any())
			ScheduledStartDate = tasks.Min(t => t!.ScheduledDate!.Value);
	}

	public DateTime? GetStartProject() { return ScheduledStartDate; }

	/// <inheritdoc/>
	public void SaveClock(DateTime time) { }

	/// <inheritdoc/>
	public DateTime LoadClock() => DateTime.Now;
}
