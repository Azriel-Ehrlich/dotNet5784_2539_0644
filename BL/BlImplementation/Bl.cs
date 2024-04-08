namespace BlImplementation;
using BlApi;
using BO;

sealed public class Bl : IBl
{
	public Bl()
	{
		// initialize fields. if we use "public IClock Clock => new ClockImplementation();" it will generate
		// too many threads and never stop (it call staop of new object and not the current thread)
		Engineer = new EngineerImplementation();
		Task = new TaskImplementation(this);
		Clock = new ClockImplementation();
	}

	public IEngineer Engineer { get; private set; }

	public ITask Task { get; private set; }

	public IClock Clock { get; private set; }

	public void InitializeDB() => DalTest.Initialization.Do();
	public void ResetDB() => DalTest.Initialization.Reset();


	/// <inheritdoc/>
	public DateTime? SuggestedDate(BO.TaskInList? task, DateTime startProj)
	{
		return SuggestedDate(task!.Id, startProj);
	}
	// like the second SuggestedDate, just get ID instead of TaskInList
	public DateTime? SuggestedDate(int tid, DateTime startProj)
	{
		BO.Task task = Task.Read(tid);
		DateTime? date = startProj;
		if (task!.Dependencies is null || task!.Dependencies.Count == 0) // there are no dependencies
			return date;

		foreach (var d in task!.Dependencies!)
		{
			BO.Task dep = Task.Read(d.Id);

			if (dep.ScheduledDate is null) // should find `ScheduledDate` for `dep`
			{
				DateTime? date2 = SuggestedDate(dep.Id, startProj);
				if (date2 is not null)
				{
					Task.UpdateScheduledDate(dep.Id, date2.Value);
					dep = Task.Read(d.Id);
				}
			}

			DateTime? depSDate = dep.ScheduledDate + dep.RequiredEffortTime;
			DateTime? depTDate = dep.StartDate + dep.RequiredEffortTime;
			if (depSDate > date)
				date = depSDate;
			if (depTDate > date && depTDate is not null)
				date = depTDate;
		}

		return date;
	}

	/// <inheritdoc/>
	public bool IsProjectScheduled()
	{
		IEnumerable<TaskInList> tasks = Task.ReadAll();
		if (tasks.Count() == 0)
			return false;

		foreach (var task in tasks)
		{
			if (task.Status == Status.Unscheduled)
				return false;
		}
		return true;
	}


	/// <inheritdoc/>
	public void MakeSuggestedDates(DateTime startProj)
	{
		foreach (var t in Task.ReadAll())
		{
			DateTime? date = SuggestedDate(t, startProj);
			if (date is not null)
			{
				Task.UpdateScheduledDate(t.Id, date.Value);
			}
		}
	}

	/// <inheritdoc/>
	public void SaveScheduledDate()
	{
		DalApi.Factory.Get.SaveScheduledDate();
	}
	/// <inheritdoc/>
	public DateTime? GetStartProject()
	{
		return DalApi.Factory.Get.GetStartProject();
	}
}
