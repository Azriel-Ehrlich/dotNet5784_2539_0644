namespace BlApi;

public interface IEngineer
{
	/// <summary> Create a new engineer </summary>
	/// <param name="engineer"> The engineer to create </param>
	/// <returns> The id of the created engineer </returns>
	public int Create(BO.Engineer engineer);

	/// <summary> Read an engineer </summary>
	/// <param name="id"> The id of the engineer to read </param>
	/// <returns> The engineer with the given id </returns>
	public BO.Engineer? ReadEngineer(int id);

	/// <summary> Read all engineers </summary>
	/// <param name="filter"> A filter to apply to the engineers </param>
	/// <returns> All engineers that pass the filter </returns>
	public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null);

	/// <summary> Update an engineer </summary>
	/// <param name="engineer"> The engineer to update </param>
	public void Update(BO.Engineer engineer);

	/// <summary> Delete an engineer </summary>
	/// <param name="id"> The id of the engineer to delete </param>
	public void Delete(int id);

	// public IEnumerable<BO.EngineerInTask> ReadAllEngineersInTask(int taskId);
}
