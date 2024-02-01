using BO;
using DalApi;
using System.Reflection;

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
			// TODO: Status-done
			// Dependencies: see below
			// TODO: Milestone
			RequiredEffortTime = task.RequiredEffortTime ?? default,
			StartDate = task.StartDate ?? default,
			ScheduledDate = task.ScheduledDate ?? default,
			// TODO: ForecastDate-will be updated after all tasks are made
			//DeadlineDate = task.DeadLineDate ?? default,
			CompleteDate = task.CompleteDate ?? default,
			Deliverables = task.Deliverables ?? "",
			Remarks = task.Remarks ?? "",
			// Engineer: see below
			Copmlexity = (BO.EngineerExperience)task.Complexity!
		};

		// find all dependencies of the task
		boTask.Dependencies = dal.Dependency.ReadAll(d => d.DependsOnTask == task.Id)
			.Where(d => d is not null)
			.Select(d =>
			{
				DO.Task t = dal.Task.Read((int)d!.DependsOnTask!) ?? throw new BO.BlDoesNotExistException($"Task with id {d.DependsOnTask} doesn't exist");
				BO.Status stat = BO.Status.Unscheduled;
				if(t.ScheduledDate is not null&& t.StartDate is null) stat = BO.Status.Scheduled;
				if(t.StartDate is not null &&t.CompleteDate is null) stat = BO.Status.OnTrack;
				if(t.CompleteDate is not null) stat = BO.Status.Done;
				return new BO.TaskInList()
				{
					Id = t.Id,
					Alias = t.Alias,
					Description = t.Description,
					Status = stat
				};
			}).ToList();

		// find the engineer assigned to the task
		DO.Engineer? eng = dal.Engineer.Read((int)task.EngineerId!) ?? throw new BO.BlDoesNotExistException($"Engineer with id {task.EngineerId} doesn't exist");
		boTask.Engineer = new BO.EngineerInTask() { Id = eng.Id, Name = eng.Name };

		//initialize status
		if (task.ScheduledDate is null) boTask.Status = BO.Status.Unscheduled;
		else// if there is schdeduled date
		{
			if ( task.StartDate is null) boTask.Status= BO.Status.Scheduled;
			if(task.StartDate is not null&& task.CompleteDate is null)boTask.Status= BO.Status.OnTrack;
			if (task.CompleteDate is not null) boTask.Status = BO.Status.Done;
		}

		return boTask;
	}

	/// <summary> Cast a BO.Task to a DO.Task. </summary>
	/// <param name="task"> The BO.Task to cast. </param>
	/// <returns> The DO.Task. </returns>
	public static DO.Task ToDOTask(this BO.Task task)
	{
		return new DO.Task(task.Id, task.Alias, task.Description, task.CreatedAtDate,
						task.RequiredEffortTime, false, (DO.EngineerExperience)task.Copmlexity!, task.StartDate,
						task.ScheduledDate,null, task.CompleteDate, task.Deliverables, task.Remarks,
						task.Engineer!.Id, true);
	}


	/// <summary> Extension method for casting generic object to string (reflection). </summary>
	/// <typeparam name="T"> The type of the object. </typeparam>
	/// <param name="obj"> The object to cast. </param>
	/// <param name="indentationLevel"> The indentation level. </param>
	/// <returns> The string representation of the object. </returns>
	public static string ToStringProperty<T>(this T obj, int indentationLevel = 0)
	{
		if (obj == null)
			return string.Empty;

		// The simple way: return obj as json
		//return JsonSerializer.Serialize(obj, new JsonSerializerOptions(){WriteIndented = true});
		// However, you said we must use reflection, so here we go:

		Type type = obj.GetType();
		string result = type.Name + " {\n";
		string indentation = new string('\t', indentationLevel);

		if (obj is System.Collections.IEnumerable lst) // the list have special case :)
		{
			foreach (var i in lst)
				result += i.ToStringProperty(indentationLevel + 1) + ",\n";
		}
		else // all other cases
		{
			result = indentation + result;

			foreach (PropertyInfo property in type.GetProperties())
			{
				object value = property!.GetValue(obj)!;
				result += $"{indentation}\t{property.Name} = ";

				if (property.PropertyType.IsPrimitive || property.PropertyType.IsValueType || property.PropertyType == typeof(string))
					result += $"{value}";
				else
					result += value.ToStringProperty(indentationLevel + 1);

				result += ",\n";
			}
		}

		result = result.Remove(result.Length - 2, 1); // remove the last comma
		result += indentation + "}";

		return result;
	}
}
