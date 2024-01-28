namespace Dal;
using DalApi;
using System.Xml.Linq;

sealed internal class DalXml : IDal
{
	// see DalList for explanation:
	private static readonly Lazy<DalXml> lazy = new Lazy<DalXml>(() => new DalXml(), LazyThreadSafetyMode.ExecutionAndPublication);
	public static DalXml Instance { get => lazy.Value; }
	private DalXml() { }

	public ITask Task => new TaskImplementation();

	public IEngineer Engineer => new EngineerImplementation();

	public IDependency Dependency => new DependencyImplementation();

	/// <summary> Reset all files to empty </summary>
	public void Reset()
	{
		// to reset the files, we just save empty lists to them:
		XMLTools.SaveListToXMLElement(new XElement("ArrayOfTask"), TaskImplementation.s_tasks_xml);
		XMLTools.SaveListToXMLElement(new XElement("ArrayOfEngineer"), EngineerImplementation.s_engineers_xml);
		XMLTools.SaveListToXMLElement(new XElement("ArrayOfDependency"), DependencyImplementation.s_dependencies_xml);
		XMLTools.SaveListToXMLElement(new XElement("config", new XElement("NextTaskId", 0), new XElement("NextDependencyId", 0)), Config.s_data_config_xml);
	}
}
