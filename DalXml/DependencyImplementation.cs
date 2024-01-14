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
		Dependency dep = item with { Id = Config.NextDependencyId};

		// read all dependencies from xml file
		XElement dependencies = XMLTools.LoadListFromXMLElement("dependencies");

		// add the new dependency to the list
		XElement xDep = new XElement("Dependency");
		xDep.Add("Id", dep.Id);
		xDep.Add("DependentTask", dep.DependentTask);
		xDep.Add("DependsOnTask", dep.DependsOnTask);
		dependencies.Add(xDep);

		// save the list to xml file
		XMLTools.SaveListToXMLElement(dependencies, "dependencies");

		return dep.Id;
	}

	public void Delete(int id)
	{
		// read all dependencies from xml file
		XElement dependencies = XMLTools.LoadListFromXMLElement("dependencies");
		// remove the dependency with the given id
		dependencies.Elements().Where(x => int.Parse(x.Element("Id")!.Value) == id).Remove();
		// save the list to xml file
		XMLTools.SaveListToXMLElement(dependencies, "dependencies");
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
		// read all dependencies from xml file
		XElement dependencies = XMLTools.LoadListFromXMLElement("dependencies");

		// convert the xml elements to dependencies
		foreach (XElement xDep in dependencies.Elements())
		{
			Dependency dep = new Dependency() {
				Id = int.Parse(xDep.Element("Id")!.Value),
				DependentTask = int.Parse(xDep.Element("DependentTask")!.Value),
				DependsOnTask = int.Parse(xDep.Element("DependsOnTask")!.Value)
			};

			// if there is a filter, check if the dependency matches it
			if (filter == null || filter(dep))
				yield return dep;
		}

		yield break;
	}

	public void Update(Dependency item)
	{
		throw new NotImplementedException();
	}
}
