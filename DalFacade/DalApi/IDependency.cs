namespace DalApi;
using DO;
public interface IDependency
{
	/// <summary>
	/// Creates new entity object in DAL
	/// </summary>
	/// <param name="item">The dependency to add</param>
	/// <returns>The new dependency's id</returns>
	int Create(Dependency item);

	/// <summary>
	/// Reads entity object by its ID 
	/// </summary>
	/// <param name="id">The dependency's id</param>
	/// <returns>The dependency with the given id</returns>
	Dependency? Read(int id);

	/// <summary>
	/// Reads all entity objects
	/// </summary>
	/// <returns>List of all dependencys</returns>
	List<Dependency> ReadAll();

	/// <summary>
	/// Updates entity object (by id)
	/// </summary>
	/// <param name="item">The new item</param>
	void Update(Dependency item);

	/// <summary>
	/// Deletes an object by its id
	/// </summary>
	/// <param name="id">The id of the item we want to delete</param>
	void Delete(int id);
}
