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
		BO.Task tas = Task.Read(task!.Id);
		DateTime? date = startProj;
		if (tas!.Dependencies is null) return date;
		foreach (var dep in tas!.Dependencies!)
		{
			BO.Task depTask = Task.Read(dep.Id);
			if (depTask!.ScheduledDate is null) throw new BlCannotUpdateException("The task cann't be updated");
			DateTime? depSDate = depTask.ScheduledDate + depTask.RequiredEffortTime;
			DateTime? depTDate = depTask.StartDate + depTask.RequiredEffortTime;
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
		foreach (var task in Task.ReadAll())
		{
			if (task.Status == Status.Unscheduled)
				return false;
		}
		return true;
	}


	/// <inheritdoc/>
	public void MakeSuggestedDates(DateTime startProj)
	{
		foreach (var t in Task.ReadAll(t => t.Dependencies is null))
		{
			Task.UpdateScheduledDate(t.Id, startProj);
		}
		foreach (var t in Task.ReadAll(t => t.Dependencies is not null))
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
}
