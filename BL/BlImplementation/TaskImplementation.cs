namespace BlImplementation;

using BO;

internal class TaskImplementation : BlApi.ITask
{
	private DalApi.IDal _dal = DalApi.Factory.Get;

	/// <summary> Check if the task is valid. </summary>
	/// <param name="task"> The task to check. </param>
	/// <exception cref="BO.BlInvalidParameterException"></exception>
	/// <exception cref="BO.BlDoesNotExistException"></exception>
	void checkTask(BO.Task task)
	{
		//if (task.Id < 0)
		//    throw new BO.BlInvalidParameterException("Id cannot be negative");

		if (string.IsNullOrEmpty(task.Alias))
			throw new BO.BlInvalidParameterException("Alias cannot be empty");

		if (task.Engineer is not null && task.Engineer.Id > 0)
		{
			DO.Engineer? eng = _dal.Engineer.Read(task.Engineer.Id);
			if (eng is null)
				throw new BO.BlDoesNotExistException($"Engineer with id={task.Engineer.Id} doesn't exist");
		}
	}

	private readonly Bl _bl;
	internal TaskImplementation(Bl bl) => _bl = bl;


	/// <inheritdoc/>
	public int Create(BO.Task task)
	{
		if (_bl.IsProjectScheduled()) // DO NOT CREATE ANY TASK IF WE DO NOT SCHEDULED OUR PROJECT!
			throw new BO.BlCannotCreateException("You can't create task before scheduled project");

		checkTask(task);

		// check if task already exist by check it alias
		DO.Task? check = (from t in _dal.Task.ReadAll()
						  where t.Alias == task.Alias
						  select t).FirstOrDefault();
		if (check is not null)
			throw new BO.BlAlreadyExistsException("The task already exist");

		if (task.Dependencies is null) // if we don't have dependencies just create the task
			return _dal.Task.Create(task.ToDOTask() with { CreatedAtDate = _bl.Clock.CurrentTime, Active = true });

		// TODO: move next line to `checkTask`
		CheckCyclicDependency(task); // In case of cyclic dependency, exception will be thrown

		int newId = _dal.Task.Create(task.ToDOTask() with { CreatedAtDate = _bl.Clock.CurrentTime, Active = true });

		// find the dependencies and create them in the DAL
		foreach (var t in task.Dependencies)
		{
			if (_dal.Task.Read(t.Id) is not null)
			{
				_dal.Dependency.Create(new DO.Dependency(newId, t.Id));
			}
		}

		return newId;
	}

	/// <inheritdoc/>
	public void Delete(int id)
	{
		if (_bl.IsProjectScheduled()) // DO NOT DELETE ANY TASK IF WE DO NOT SCHEDULED OUR PROJECT!
			throw new BlCannotDeleteException("You can't delete task after scheduled project");

		// check if there is another task teat depends on this task
		DO.Dependency? check = (from t in _dal.Dependency.ReadAll()
								where t.DependsOnTask == id
								select t).FirstOrDefault();
		if (check is not null) throw new BO.BlCannotDeleteException("The task cann't be deleted because there is another task that depends on it");

		try
		{
			_dal.Task.Delete(id);
		}
		catch (DO.DalDoesNotExistException ex)
		{
			throw new BO.BlDoesNotExistException(ex.Message);
		}
	}

	/// <inheritdoc/>
	public void Restore(int id)
	{
		BO.Task task = Read(id);
		task.IsActive = true;
		Update(task);
	}

	/// <inheritdoc/>
	public BO.Task Read(int id)
	{
		DO.Task task = _dal.Task.Read(id) ?? throw new BO.BlDoesNotExistException("The task doesn't exist");
		return task.ToBOTask(_dal);
	}

	/// <inheritdoc/>
	public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null)
	{
		IEnumerable<DO.Task?> res;

		if (filter is null)
			res = _dal.Task.ReadAll();
		else
			res = _dal.Task.ReadAll(t => filter(t.ToBOTask(_dal)));

		IEnumerable<BO.Task> conv = res.Where(t => t is not null).Select(t => t!.ToBOTask(_dal));
		return from t in conv select (new BO.TaskInList() { Id = t.Id, Description = t.Description, Alias = t.Alias, Status = t.Status });
	}

	/// <summary> Get the dependencies of a task. </summary>
	/// <param name="taskId"> The id of the task. </param>
	/// <returns> The list of the dependencies' ids. </returns>
	List<int> GetDependencies(int taskId)
	{
		BO.Task task = Read(taskId);
		return (task.Dependencies is null) ? new() : task.Dependencies.Select(t => t.Id).ToList();
	}

	/// <summary> Check if there is a cyclic dependency. </summary>
	/// <param name="task"> The task to check. </param>
	/// <exception cref="BO.BlCannotUpdateException"> If there is a cyclic dependency. </exception>
	void CheckCyclicDependency(BO.Task task)
	{
		/*
		TODO: improve this method:

		this case:
		   b <- (a)
		   c <- (b, a)

		is not a cyclic dependency, but the current implementation will throw an exception

		*/

		if (task.Dependencies is not null)
		{
			List<int> deps = (task.Dependencies is null) ? new() : task.Dependencies.Select(t => t.Id).ToList();
			if (task.Id != -1) // it is not a new task
				deps.Add(task.Id);
			for (int i = 0; i < deps.Count; i++)
			{
				List<int> newDeps = GetDependencies(deps[i]);

				foreach (int j in newDeps)
				{
					// check if the new dependency is already in the list
					// we check also here because in case of circular dependency we have endless loop
					if (deps.Contains(j))
						throw new BO.BlCannotUpdateException("Circular dependency");

					// add the dependencies of the dependent task
					deps.Add(j);
				}
			}

			// check for duplicate values in list
			bool hasDuplicates = (from t in deps
								  group t by t into g
								  where g.Count() > 1
								  select g).Any();
			if (hasDuplicates)
				throw new BO.BlCannotUpdateException("Circular dependency");
		}
	}

	/// <inheritdoc/>
	public void Update(BO.Task task)
	{
		checkTask(task);

		DO.Task? check = (from t in _dal.Task.ReadAll()
						  where t.Id == task.Id
						  select t).FirstOrDefault();
		if (check is null) throw new BO.BlDoesNotExistException("The task doesn't exist");


		if (task.Dependencies is not null)
		{

			IEnumerable<DO.Dependency?> test = _dal.Dependency.ReadAll(d => d.DependentTask == task.Id);

			//we don't need to check cyclic dpendency if we  delete a dependency
			if (task.Dependencies.Count() > test.Count())
			{
				CheckCyclicDependency(task); // In case of cyclic dependency, exception will be thrown
			}

			// delete all old dependencies
			IEnumerable<DO.Dependency?> oldDeps = (_dal.Dependency.ReadAll(d => d.DependentTask == task.Id)).ToList();
			foreach (var dep in oldDeps)
			{
				_dal.Dependency.Delete(dep!.Id);
			}

			// add the new dependencies
			foreach (var dep in task.Dependencies)
			{
				DO.Task? checkDep = _dal.Task.Read(dep.Id);
				if (checkDep is null) continue;

				BO.Task? checkDepBO = checkDep.ToBOTask(_dal);
				if (checkDepBO is null) continue;

				/*
                if (
                    (checkDepBO.StartDate is not null && checkDepBO.StartDate > task.StartDate) ||// check if the dependent task start after the task
                    (checkDepBO.CompleteDate is not null && checkDepBO.CompleteDate > task.StartDate) ||// check if the dependent task complete after the task
                    (checkDepBO.ForecastDate is not null && checkDepBO.ForecastDate > task.StartDate)// check if the dependent task forecast after the task
                    )
                    throw new BlCannotUpdateException("Dependent task cann't start before the task");
               */
				_dal.Dependency.Create(new DO.Dependency(task.Id, dep.Id));
			}
		}

		_dal.Task.Update(task.ToDOTask());
	}

	/// <inheritdoc/>
	public void StartTask(int taskId, int engId)
	{
		if (!_bl.IsProjectScheduled()) // DO NOT START ANY TASK IF WE DO NOT SCHEDULED OUR PROJECT!
			throw new BlCannotUpdateException("You can't start task before scheduled project");

		BO.Task task = Read(taskId);

		if (task.Engineer != null)
			throw new BlCannotUpdateException("another engineer already assign to this task");

		// check if engineer is available:
		if (_bl.Engineer.Read(engId).Task is not null)
			throw new BlCannotUpdateException("Engineer is busy at this time");

		// check dependencies if we can start:
		if (task.Dependencies is not null)
		{
			foreach (var t in task.Dependencies)
			{
				if (t.Status != Status.Done)
					throw new BlCannotUpdateException("You must finish task's dependencies first.");
			}
		}

		task.StartDate = _bl.Clock.CurrentTime;
		BO.Engineer eng = _bl.Engineer.Read(engId);
		task.Engineer = new EngineerInTask() { Id = engId, Name = eng.Name };
		Update(task);
	}

	/// <inheritdoc/>
	public void FinishTask(int id)
	{
		BO.Task task = Read(id);

		if (task.StartDate == null)
			throw new BlCannotUpdateException("you need start task to finish it");

		if (task.Engineer == null)
			throw new BlCannotUpdateException("you need assign engineer to task to finish it");

		task.CompleteDate = _bl.Clock.CurrentTime;
		Update(task);
	}

	/// <inheritdoc/>
	public void UpdateScheduledDate(int id, DateTime date)
	{
		DO.Task? tas = _dal.Task.Read(id);
		if (tas is null) throw new BO.BlDoesNotExistException("The task doesn't exist");

		IEnumerable<DO.Dependency?> test = _dal.Dependency.ReadAll(d => d.DependentTask == id);
		IEnumerable<DO.Task?> check = from t in test
									  where t.DependentTask == id
									  select _dal.Task.Read((int)t.DependsOnTask!);
		if ((from t in check where t.ScheduledDate == null select t).Any()) throw new BO.BlCannotUpdateException($"The task {tas.Id} cann't be updated");
		if ((from t in check
			 where (t.ScheduledDate + t.RequiredEffortTime) > date ||
			 ((t.StartDate + t.RequiredEffortTime) > date && t.StartDate is not null)
			 select t).Any())
		{
			throw new BO.BlCannotUpdateException($"The task {tas.Id} cann't be updated");
		}
		_dal.Task.Update(tas with { ScheduledDate = date });
	}
}
