namespace Dal;

using DO;
using DalApi;
using System.Collections.Generic;

public class TaskImplementation : ITask
{
	public int Create(Task item)
	{
		Task newTask = item with { Id = DataSource.Config.NextTaskId };
		DataSource.Tasks.Add(newTask);
		return newTask.Id;
	}

	public void Delete(int id)
	{
		Task? task = Read(id) ?? throw new Exception($"Task with the same id doesn't exist: id={id}");
		Update(task with { Active = false }); // we don't want to remove the task, just make it inactive
	}

	public Task? Read(int id)
	{
		return DataSource.Tasks.Find(x => x.Id == id);
	}

	public List<Task> ReadAll()
	{
		return new List<Task>(DataSource.Tasks);
	}

	public void Update(Task item)
	{
		Task? task = Read(item.Id) ?? throw new Exception($"Task with the same id doesn't exist: id={item.Id}");
		DataSource.Tasks.Remove(task); // remove the old item
		DataSource.Tasks.Add(item); // and add the new one
	}
}
