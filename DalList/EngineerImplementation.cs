namespace Dal;

using DO;
using DalApi;
using System.Collections.Generic;

public class EngineerImplementation : IEngineer
{
	public int Create(Engineer item)
	{
		// search if `item` already exists
		if (Read(item.Id) is not null)
			throw new Exception($"Engineer with the same id already exists: id={item.Id}");

		DataSource.Engineers.Add(item);
		return (int)item.Id;
	}

	public void Delete(int id)
	{
		Engineer? eng = Read(id) ?? throw new Exception($"Engineer with the same id doesn't exist: id={id}");
		//DataSource.Engineers.Remove(eng);
		Update(eng with { Active = false });
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
