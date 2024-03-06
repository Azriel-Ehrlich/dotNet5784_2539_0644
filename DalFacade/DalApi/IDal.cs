namespace DalApi;

public interface IDal
{
	ITask Task { get; }
	IEngineer Engineer { get; }
	IDependency Dependency { get; }

	/// <summary> Reset the entire database </summary>
	abstract void Reset();

	/// <summary> Saves the start and end date of the project </summary>
	abstract void SaveScheduledDate();

	/// <summary> Returns the start date of the project </summary>
	abstract DateTime? GetStartProject();

	/// <summary> save clock to DB </summary>
	public void SaveClock(DateTime time);
	/// <summary> load clock from DB </summary>
	public DateTime LoadClock();
}
