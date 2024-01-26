using DalApi;

namespace BlImplementation;


/// <summary> This class contains extension methods for casting between DO and BO. </summary>
internal static class Tools
{
	/// <summary> Cast a DO.Engineer to a BO.Engineer. </summary>
	/// <param name="engineer"> The DO.Engineer to cast. </param>
	/// <param name="dal"> The dal to use for the cast. </param>
	/// <returns> The BO.Engineer. </returns>
	public static BO.Engineer ToBOEngineer(this DO.Engineer engineer, IDal dal)
	{
		BO.Engineer boEng = new BO.Engineer()
		{
			Id = engineer.Id,
			Email = engineer.Email,
			Cost = engineer.Cost,
			Name = engineer.Name,
			Level = (BO.EngineerExperience)engineer.Level,
			Active = engineer.Active
		};

		// find all tasks that the engineer is assigned to
		boEng.Task = dal.Task.ReadAll(t => t.EngineerId == engineer.Id)
			.Where(t => t is not null)
			.Select(t => new BO.TaskInEngineer() { Id = t!.Id, Alias = t.Alias })
			.ToList();

		return boEng;
	}

	/// <summary> Cast a BO.Engineer to a DO.Engineer. </summary>
	/// <param name="engineer"> The BO.Engineer to cast. </param>
	/// <returns> The DO.Engineer. </returns>
	public static DO.Engineer ToDOEngineer(this BO.Engineer engineer)
	{
		return new DO.Engineer(engineer.Id, engineer.Email, engineer.Cost,
			engineer.Name, (DO.EngineerExperience)engineer.Level);
	}

	/// <summary> Cast a DO.Task to a BO.Task. </summary>
	/// <param name="task"> The DO.Task to cast. </param>
	/// <param name="dal"> The dal to use for the cast. </param>
	/// <returns> The BO.Task. </returns>
	public static BO.Task ToBOTask(this DO.Task task, IDal dal)
	{
		BO.Task boTask = new BO.Task()
		{
			Id = task.Id,
			Alias = task.Alias,
			Description = task.Description,
			CreatedAtDate = task.CreatedAtDate,
			// TODO: Status
			// TODO: Dependencies
			// TODO: Milestone
			RequiredEffortTime = task.RequiredEffortTime ?? default,
			StartDate = task.StartDate ?? default,
			ScheduledDate = task.ScheduledDate ?? default,
			// TODO: ForecastDate,
			DeadlineDate = task.DeadLineDate ?? default,
			CompleteDate = task.CompleteDate ?? default,
			Deliverables = task.Deliverables,
			Remarks = task.Remarks,
			// TODO: Engineer
			Copmlexity = (BO.EngineerExperience)task.Complexity
		};

		// find all dependencies of the task
		boTask.Dependencies = dal.Dependency.ReadAll(d => d.DependsOnTask == task.Id)
			.Where(d => d is not null)
			.Select(d =>
			{
				DO.Task t = dal.Task.Read((int)d.DependsOnTask!) ?? throw new BO.BlDoesNotExistException($"Task with id {d.DependsOnTask} doesn't exist");
				return new BO.TaskInList()
				{
					Id = t.Id,
					Alias = t.Alias,
					Description = t.Description,
					// TODO: Status
				};
			}).ToList();

		// find the engineer assigned to the task
		DO.Engineer? eng = dal.Engineer.Read((int)task.EngineerId!) ?? throw new BO.BlDoesNotExistException($"Engineer with id {task.EngineerId} doesn't exist");
		boTask.Engineer = new BO.EngineerInTask() { Id = eng.Id, Name = eng.Name };

		return boTask;
	}
}
