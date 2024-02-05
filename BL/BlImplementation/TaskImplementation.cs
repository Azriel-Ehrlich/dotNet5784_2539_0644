namespace BlImplementation;

using BO;

internal class TaskImplementation : BlApi.ITask
{
	private DalApi.IDal _dal = DalApi.Factory.Get;

	void checkTask(BO.Task task)
	{
		if (task.Id < 0) throw new BO.BlInvalidParameterException("Id cannot be negative");
		if (string.IsNullOrEmpty(task.Alias)) throw new BO.BlInvalidParameterException("Alias cannot be empty");
	}


	/// <inheritdoc/>
	public int Create(BO.Task task)
	{
		checkTask(task);

		// check if task already exist by check it alias
		DO.Task? check = (from t in _dal.Task.ReadAll()
						  where t.Alias == task.Alias
						  select t).FirstOrDefault();
		if (check is not null) throw new BO.BlAlreadyExistsException("The task already exist");

		int newId = _dal.Task.Create(task.ToDOTask() with { CreatedAtDate = DateTime.Now });

		if (task.Dependencies is not null)
		{
			// find the dependencies and create them in the DAL
			// (we save `deps` so the complier doesn't yell at us)
			/*
			var deps = from t in task.Dependencies
					   where _dal.Task.Read(t.Id) is not null
					   select _dal.Dependency.Create(new DO.Dependency(newId, t.Id));
			*/
			// TODO: thats not works using linq, use foreach instead
			foreach (var t in task.Dependencies)
			{
				if (_dal.Task.Read(t.Id) is not null)
				{
					_dal.Dependency.Create(new DO.Dependency(newId, t.Id));
				}
			}
		}
		return newId;
	}

	/// <inheritdoc/>
	public void Delete(int id)
	{
		// check if there is another task teat depends on this task
		DO.Dependency? check = (from t in _dal.Dependency.ReadAll()
								where t.DependsOnTask == id
								select t).FirstOrDefault();
		if (check is not null) throw new BO.BlCannotDeleteException("The task cann't be deleted");

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

	/// <inheritdoc/>
	public void Update(BO.Task task)
	{
		DO.Task? check = (from t in _dal.Task.ReadAll()
						  where t.Id == task.Id
						  select t).FirstOrDefault();
		if (check is null) throw new BO.BlDoesNotExistException("The task doesn't exist");

		if (task.StartDate is not null)
		{
			List<TaskInList>? deps = task.Dependencies;
			if (deps is not null)
			{
				foreach (var dep in deps)
				{
					DO.Task? checkDep = _dal.Task.Read(dep.Id);
					if (checkDep is null) continue;

					BO.Task? checkDepBO = checkDep.ToBOTask(_dal);
					if (checkDepBO is null) continue;

					if (
						(checkDepBO.StartDate is not null && checkDepBO.StartDate > task.StartDate) ||// check if the dependent task start after the task
						(checkDepBO.CompleteDate is not null && checkDepBO.CompleteDate > task.StartDate) ||// check if the dependent task complete after the task
						(checkDepBO.ForecastDate is not null && checkDepBO.ForecastDate > task.StartDate)// check if the dependent task forecast after the task
						)
						throw new BlCannotUpdateException("Dependent task cann't start before the task");
					_dal.Dependency.Update(new DO.Dependency(task.Id, dep.Id));
				}
			}
		}

		_dal.Task.Update(task.ToDOTask());
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
		_dal.Task.Update(tas with { ScheduledDate = date});
	}
}
