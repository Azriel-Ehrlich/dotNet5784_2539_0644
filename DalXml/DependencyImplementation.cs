namespace Dal;
using DO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
	readonly string s_dependencies_xml = "dependencies";

	/// <inheritdoc/>
	public int Create(Dependency item)
	{
		// read all dependencies from xml file
		XElement dependencies = XMLTools.LoadListFromXMLElement(s_dependencies_xml);

		// add the new dependency to the list
		int id = Config.NextDependencyId;
		dependencies.Add(new XElement("Dependency",
			new XElement("Id", id),
			new XElement("DependentTask", item.DependentTask),
			new XElement("DependsOnTask", item.DependsOnTask)
		));

		// save the list to xml file
		XMLTools.SaveListToXMLElement(dependencies, s_dependencies_xml);

		return id;
	}

	/// <inheritdoc/>
	public void Delete(int id)
	{
		// read all dependencies from xml file
		XElement dependencies = XMLTools.LoadListFromXMLElement(s_dependencies_xml);

		// remove the dependency with the given id
		XElement xDep = GetXmlDependencyById(dependencies, id) ?? throw new DalDoesNotExistException($"Dependency with id {id} doesn't exist");
		xDep.Remove();

		// save the list to xml file
		XMLTools.SaveListToXMLElement(dependencies, s_dependencies_xml);
	}

	/// <inheritdoc/>
	public Dependency? Read(int id)
	{
		return Read(dep => dep.Id == id); // use the other Read method
	}

	/// <inheritdoc/>
	public Dependency? Read(Func<Dependency, bool> filter)
	{
		return ReadAll(filter).FirstOrDefault(); // we did it, so we use it!
	}

	/// <inheritdoc/>
	public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
	{
		IEnumerable<Dependency> list = XMLTools.LoadListFromXMLElement(s_dependencies_xml)
			.Elements()
			.Select(ConvertXmlToDependency);

		if (filter != null)
			list = list.Where(filter);

		return list;
	}

	/// <inheritdoc/>
	public void Update(Dependency item)
	{
		// read all dependencies from xml file
		XElement dependencies = XMLTools.LoadListFromXMLElement(s_dependencies_xml);

		// find child with the given id
		XElement xDep = GetXmlDependencyById(dependencies, item.Id) ?? throw new DalDoesNotExistException($"Dependency with id {item.Id} doesn't exist");

		// update the dependency
		xDep.Element("DependentTask")!.Value = item.DependentTask.ToString()!;
		xDep.Element("DependsOnTask")!.Value = item.DependsOnTask.ToString()!;

		// save the list to xml file
		XMLTools.SaveListToXMLElement(dependencies, s_dependencies_xml);
	}


	/// <summary> Get an XElement from a parent XElement by the id of the dependency </summary>
	/// <param name="parent"> The parent XElement </param>
	/// <param name="id"> The id of the dependency </param>
	/// <returns> The XElement with the given id </returns>
	private XElement? GetXmlDependencyById(XElement parent, int id)
	{
		return parent.Elements()
			.Where(x => ConvertXmlToDependency(x).Id == id)
			.FirstOrDefault();
	}

	/// <summary> Convert an XElement to a Dependency </summary>
	/// <param name="xDep"> The XElement to convert </param>
	/// <returns> The converted Dependency </returns>
	private Dependency ConvertXmlToDependency(XElement xDep)
	{
		return new Dependency(
			(int)xDep.ToIntNullable("Id")!,
			xDep.ToIntNullable("DependentTask"),
			xDep.ToIntNullable("DependsOnTask")
		);
	}
}
