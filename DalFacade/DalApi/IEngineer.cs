namespace DalApi;
using DO;
public interface IEngineer
{
	/// <summary>
	/// Creates new entity object in DAL
	/// </summary>
	/// <param name="item">The engineer to add</param>
	/// <returns>The new engineer's id</returns>
	int Create(Engineer item);

	/// <summary>
	/// Reads entity object by its ID 
	/// </summary>
	/// <param name="id">The engineer's id</param>
	/// <returns>The engineer with the given id</returns>
	Engineer? Read(int id);

	/// <summary>
	/// Reads all entity objects
	/// </summary>
	/// <returns>List of all engineers</returns>
	List<Engineer> ReadAll();

	/// <summary>
	/// Updates entity object (by id)
	/// </summary>
	/// <param name="item">The new item</param>
	void Update(Engineer item);

	/// <summary>
	/// Deletes an object by its id
	/// </summary>
	/// <param name="id">The id of the item we want to delete</param>
	void Delete(int id);
}
