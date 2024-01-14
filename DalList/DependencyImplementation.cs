namespace Dal;

using DO;
using DalApi;
using System.Collections.Generic;

internal class DependencyImplementation : IDependency
{
	public int Create(Dependency item)
	{
		Dependency newDep = item with { Id = DataSource.Config.NextDependencyId };
		DataSource.Dependencies.Add(newDep);
		return newDep.Id;
	}

	public void Delete(int id)
	{
		Dependency? dep = Read(id) ?? throw new DalDoesNotExistException($"Dependency with id {id} doesn't exist");
		DataSource.Dependencies.Remove(dep);
	}

	public Dependency? Read(int id)
	{
		return Read(x => x.Id == id);
	}

	public Dependency? Read(Func<Dependency, bool> filter)
	{
		return DataSource.Dependencies.Where(filter).FirstOrDefault();
	}

	public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
	{
		if (filter is null)
			return DataSource.Dependencies.Select(e => e);
		return DataSource.Dependencies.Where(filter);
	}

	public void Update(Dependency item)
	{
		Dependency? dep = Read(item.Id) ?? throw new DalDoesNotExistException($"Dependency with id {item.Id} doesn't exist");
		DataSource.Dependencies.Remove(dep); // remove the old item
		DataSource.Dependencies.Add(item); // and add the new one
	}
}
