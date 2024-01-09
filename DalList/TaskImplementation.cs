namespace Dal;

using DO;
using DalApi;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
	public int Create(Task item)
	{
		Task newTask = item with { Id = DataSource.Config.NextTaskId };
		DataSource.Tasks.Add(newTask);
		return newTask.Id;
	}

	public void Delete(int id)
	{
		Task? task = Read(id) ?? throw new DalDoesNotExistException($"Task with id {id} doesn't exist");
		Update(task with { Active = false }); // we don't want to remove the task, just make it inactive
	}

	public Task? Read(int id)
	{
		return Read(x => x.Id == id);
	}

	public Task? Read(Func<Task, bool> filter)
	{
		return DataSource.Tasks.Where(filter).FirstOrDefault();
	}

	public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
	{
		if (filter is null)
			return DataSource.Tasks.Select(e => e);
		return DataSource.Tasks.Where(filter);
	}

	public void Update(Task item)
	{
		Task? task = Read(item.Id) ?? throw new DalDoesNotExistException($"Task with id {item.Id} doesn't exist");
		DataSource.Tasks.Remove(task); // remove the old item
		DataSource.Tasks.Add(item); // and add the new one
	}
}
