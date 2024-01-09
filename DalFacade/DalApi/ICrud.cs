namespace DalApi;

public interface ICrud<T> where T : class
{
	/// <summary> Creates new entity object in DAL </summary>
	/// <param name="item">The item to add</param>
	/// <returns>The new item's id</returns>
	int Create(T item);

	/// <summary> Reads entity object by its ID  </summary>
	/// <param name="id">The objects's id</param>
	/// <returns>The object with the given id</returns>
	T? Read(int id);

    /// <summary> Reads all entity objects </summary>
    /// <returns>List of all items</returns>
    IEnumerable<T?> ReadAll(Func<T, bool>? filter = null);

    /// <summary> Updates entity object (by id) </summary>
    /// <param name="item">The new item</param>
    void Update(T item);

	/// <summary> Deletes an object by its id </summary>
	/// <param name="id">The id of the item we want to delete</param>
	void Delete(int id);
	/// <summary>
	/// read an item by a given filter not nessesary by id
	/// </summary>
	/// <param name="filter"></param>
	/// <returns></returns>
    T? Read(Func<T, bool> filter);

}

