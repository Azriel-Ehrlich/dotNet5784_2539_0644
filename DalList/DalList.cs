namespace Dal;

using DalApi;

sealed internal class DalList : IDal
{
	// safe-thread and lazy-initialization singleton: https://csharpindepth.com/articles/singleton
	private static readonly Lazy<DalList> lazy = new Lazy<DalList>(() => new DalList());
	private static readonly object padlock = new object();
	public static DalList Instance { get { lock (padlock) { return lazy.Value; } } }

	private DalList() { }

	public ITask Task => new TaskImplementation();

	public IEngineer Engineer => new EngineerImplementation();

	public IDependency Dependency => new DependencyImplementation();

	public void Reset() { }
}
