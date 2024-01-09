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
			throw new Exception($"Engineer with the same id already exists: id={item.Id}");

		DataSource.Engineers.Add(item);
		return item.Id;
	}

	public void Delete(int id)
	{
		Engineer? eng = Read(id) ?? throw new Exception($"Engineer with the same id doesn't exist: id={id}");
		Task? task = DataSource.Tasks.Find(x => x.Id == id);
		if (task is not null&&task.Active==true)
            throw new Exception($"Engineer with the same id is still assigned to a task: id={id}");
		Update(eng with { Active = false }); // we don't want to remove the engineer, just make him inactive
	}

	public Engineer? Read(int id)
	{
		return DataSource.Engineers.Find(x => x.Id == id); // just find
	}

	public List<Engineer> ReadAll()
	{
		return new List<Engineer>(DataSource.Engineers);
	}

	public void Update(Engineer item)
	{
		Engineer? eng = Read(item.Id) ?? throw new Exception($"Engineer with the same id doesn't exist: id={item.Id}");
		DataSource.Engineers.Remove(eng); // remove the old item
		DataSource.Engineers.Add(item); // and add the new one
	}
}
