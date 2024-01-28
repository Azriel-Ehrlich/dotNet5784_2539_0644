namespace BlImplementation;
using BlApi;

sealed public class Bl : IBl
{
	public IEngineer Engineer => new EngineerImplementation();

	public ITask Task => new TaskImplementation();
}
