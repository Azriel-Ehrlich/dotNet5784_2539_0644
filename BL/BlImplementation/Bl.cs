namespace BlImplementation;
using BlApi;
using BO;

sealed public class Bl : IBl
{
	public IEngineer Engineer => new EngineerImplementation();

	public ITask Task => new TaskImplementation(this);

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


	#region Clock Object

	static object s_Lock = new object(); // Lock for the clock
	bool s_runClockThread = false;
	static DateTime s_Clock = DateTime.Now.Date;

	public DateTime Clock
	{
		get { lock (s_Lock) { return s_Clock; } }
		private set { lock (s_Lock) { s_Clock = value; } }
	}

	public void InitClock() { lock (s_Lock) { Clock = DateTime.Now; } }
	public void AddHours(int hours) { lock (s_Lock) { Clock = Clock.AddHours(hours); } }
	public void AddDays(int days) { lock (s_Lock) { Clock = Clock.AddDays(days); } }
	public void AddMinutes(int minutes) { lock (s_Lock) { Clock = Clock.AddMinutes(minutes); } }
	public void AddSeconds(int seconds) { lock (s_Lock) { Clock = Clock.AddSeconds(seconds); } }

	public void StartClockThread(Action onChange)
	{
		new Thread(() =>
		{
			s_runClockThread = true;
			while (s_runClockThread)
			{
				Thread.Sleep(1000);
				AddMinutes(10);
				AddSeconds(1);
				onChange();
			}
		}).Start();
	}
	public void StopClockThread() { s_runClockThread = false; }
	#endregion
}
