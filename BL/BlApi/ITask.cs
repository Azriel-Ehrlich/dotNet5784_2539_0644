namespace BlApi;

internal interface ITask
{
	/// <summary> Create a new task </summary>
	/// <param name="task"> The task to create </param>
	/// <returns> The id of the created task </returns>
	public int Create(BO.Task task);

	/// <summary> Read a task </summary>
	/// <param name="id"> The id of the task to read </param>
	/// <returns> The task with the given id </returns>
	public BO.Task? Read(int id);

	/// <summary> Read all tasks </summary>
	/// <param name="filter"> A filter to apply to the tasks </param>
	/// <returns> All tasks that pass the filter </returns>
	public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null);

	/// <summary> Update a task </summary>
	/// <param name="task"> The task to update </param>
	public void Update(BO.Task task);

	/// <summary> Update the date of a task </summary>
	/// <param name="id"> The id of the task to update </param>
	/// <param name="date"> The new date of the task </param>
	public void UpdateDate(int id, DateTime date);
}
