namespace Dal;
using DalApi;
using DO;
using System.Xml.Linq;

public class DalXml : IDal
{
	public ITask Task => new TaskImplementation();

	public IEngineer Engineer => new EngineerImplementation();

	public IDependency Dependency => new DependencyImplementation();


	/// <summary> Reset all files to empty </summary>
	public void ResetFiles()
	{
		// to reset the files, we just save empty lists to them:
		XMLTools.SaveListToXMLElement(new XElement("ArrayOfTask"), TaskImplementation.s_tasks_xml);
		XMLTools.SaveListToXMLElement(new XElement("ArrayOfEngineer"), EngineerImplementation.s_engineers_xml);
		XMLTools.SaveListToXMLElement(new XElement("ArrayOfDependency"), DependencyImplementation.s_dependencies_xml);
		XMLTools.SaveListToXMLElement(new XElement("config", new XElement("NextTaskId", 0), new XElement("NextDependencyId", 0)), Config.s_data_config_xml);
	}
}
