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

		XMLTools.SaveListToXMLElement(
			new XElement("config",
				new XElement("NextTaskId", 0),
				new XElement("NextDependencyId", 0),
				new XElement("ScheduledStartDate")
			), Config.s_data_config_xml);
	}

	/// <inheritdoc/>
	public void SaveScheduledDate()
	{
		// get the earliest start date
		IEnumerable<DO.Task?> tasks = Task.ReadAll()
			.Where(t => t is null || t.ScheduledDate is not null);

		DateTime start = DateTime.MinValue;
		if (tasks.Any())
			start = tasks.Min(t => t!.ScheduledDate!.Value);

		// save with the global config file
		XElement config = XMLTools.LoadListFromXMLElement(Config.s_data_config_xml);

		XElement? scheduledStartDate = config.Element("ScheduledStartDate");
		if (scheduledStartDate is not null)
			scheduledStartDate.Value = start.ToString();
		else
			config.Add(new XElement("ScheduledStartDate", start));

		XMLTools.SaveListToXMLElement(config, Config.s_data_config_xml);
	}

	/// <inheritdoc/>
	public void SaveClock(DateTime time)
	{
		XElement config = XMLTools.LoadListFromXMLElement(Config.s_data_config_xml);

		XElement? clock = config.Element("Clock");
		if (clock is not null)
			clock.Value = time.ToString();
		else
			config.Add(new XElement("Clock", time));

		XMLTools.SaveListToXMLElement(config, Config.s_data_config_xml);
	}

	/// <inheritdoc/>
	public DateTime LoadClock()
	{
		XElement config = XMLTools.LoadListFromXMLElement(Config.s_data_config_xml);

		XElement? clock = config.Element("Clock");
		if (clock is not null)
			return DateTime.Parse(clock.Value);
		else
			return DateTime.Now;
	}
}
