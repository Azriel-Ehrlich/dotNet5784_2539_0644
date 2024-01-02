namespace DalApi;
using DO;
public interface ITask
{
	/// <summary>
	/// Creates new entity object in DAL
	/// </summary>
	/// <param name="item">The task to add</param>
	/// <returns>The new task's id</returns>
	int Create(Task item);

	/// <summary>
	/// Reads entity object by its ID 
	/// </summary>
	/// <param name="id">The task's id</param>
	/// <returns>The task with the given id</returns>
	Task? Read(int id);

	/// <summary>
	/// Reads all entity objects
	/// </summary>
	/// <returns>List of all tasks</returns>
	List<Task> ReadAll();

	/// <summary>
	/// Updates entity object (by id)
	/// </summary>
	/// <param name="item">The new item</param>
	void Update(Task item);

	/// <summary>
	/// Deletes an object by its id
	/// </summary>
	/// <param name="id">The id of the item we want to delete</param>
	void Delete(int id);
}
