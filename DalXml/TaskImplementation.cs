namespace Dal;
using DO;
using DalApi;
using System;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
	readonly string s_tasks_xml = "tasks";

	/// <inheritdoc/>
	public int Create(Task item)
	{
		List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
		int id = Config.NextTaskId;
		tasks.Add(item with { Id = id });
		XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
		return id;
	}

	/// <inheritdoc/>
	public void Delete(int id)
	{
		List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        Task? task = tasks.Where(t => t.Id == id).FirstOrDefault() ?? throw new DalDoesNotExistException($"Task with id {id} doesn't exist");
		Update(task with { Active = false });
	}

	/// <inheritdoc/>
	public Task? Read(int id)
	{
		return Read(x => x.Id == id);
	}

	/// <inheritdoc/>
	public Task? Read(Func<Task, bool> filter)
	{
		return ReadAll(filter).FirstOrDefault();
	}

	/// <inheritdoc/>
	public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
	{
		List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
		if (filter is null)
            return tasks.Select(t => t);
		return tasks.Where(filter);
	}

	/// <inheritdoc/>
	public void Update(Task item)
	{
		List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        Task? task = Read(item.Id) ?? throw new DalDoesNotExistException($"Task with id {item.Id} doesn't exist");
		tasks.Remove(task);
		tasks.Add(item);
		XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
	}
}
