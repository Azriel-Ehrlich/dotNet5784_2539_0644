namespace Dal;

using DO;
using DalApi;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
	public int Create(Engineer item)
	{
		// search if `item` already exists
		if (Read(item.Id) is not null)
			throw new DalAlreadyExistsException($"Engineer with id {item.Id} already exists");

		DataSource.Engineers.Add(item);
		return item.Id;
	}

	public void Delete(int id)
	{
		Engineer eng = Read(id) ?? throw new DalDoesNotExistException($"Engineer with id {id} doesn't exist");
		Task? task = DataSource.Tasks.Where(x => x.EngineerId == id).FirstOrDefault();
		if (task is not null && task.Active == true)
			throw new DalDoesNotExistException($"Engineer with id {id} is still assigned to a task");
		Update(eng with { Active = false }); // we don't want to remove the engineer, just make him inactive
	}

	public Engineer? Read(int id)
	{
		return Read(x => x.Id == id);
	}

	public Engineer? Read(Func<Engineer, bool> filter)
	{
		return DataSource.Engineers.Where(filter).FirstOrDefault(); // just find
	}

	public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
	{
		if (filter is null)
			return DataSource.Engineers.Select(e => e);
		return DataSource.Engineers.Where(filter);
	}

	public void Update(Engineer item)
	{
		Engineer? eng = Read(item.Id) ?? throw new DalDoesNotExistException($"Engineer with id {item.Id} doesn't exist");
		DataSource.Engineers.Remove(eng); // remove the old item
		DataSource.Engineers.Add(item); // and add the new one
	}
}
