namespace Dal;
using DO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
	public int Create(Dependency item)
	{
		// read all dependencies from xml file
		List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>("dependencies");
		
		// add the new dependency to the list
		Dependency dep = item with { Id = Config.NextDependencyId};
		dependencies.Add(dep);
		
		// save the list to xml file
		XMLTools.SaveListToXMLSerializer(dependencies, "dependencies");

		return dep.Id;
	}

	public void Delete(int id)
	{
		throw new NotImplementedException();
	}

	public Dependency? Read(int id)
	{
		throw new NotImplementedException();
	}

	public Dependency? Read(Func<Dependency, bool> filter)
	{
		throw new NotImplementedException();
	}

	public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
	{
		throw new NotImplementedException();
	}

	public void Update(Dependency item)
	{
		throw new NotImplementedException();
	}
}
