using System.Runtime.CompilerServices;

namespace DO;

/// <summary>
/// A task to be done by an engineer
/// </summary>
/// <param name="Id">unique id of the task</param>
/// <param name="Alias">Task's alias</param>
/// <param name="Description">Task's description</param>
/// <param name="CreatedAtDate">Date of creation</param>
/// <param name="RequiredEffortTime">Time required to complete the task</param>
/// <param name="IsMilestone">Task is a milestone</param>
/// <param name="Complexity">The complexity of the task</param>
/// <param name="StartDate">Actual start date</param>
/// <param name="SchedualdDate">Date the task is scheduled to start</param>
/// <param name="DeadLineDate">Date the task is scheduled to end</param>
/// <param name="CompleteDate">Actual end date</param>
/// <param name="Deliverables">Task's deliverables</param>
/// <param name="Remarks">Remarks about the task</param>
/// <param name="EngineerId">Id of the engineer assigned to the task</param>
public record Task
(
	int Id,
	string Alias,
	string Description,
	DateTime CreatedAtDate,
	TimeSpan? RequiredEffortTime = null,
	bool IsMilestone = false,
	EngineerExperience? Complexity = null,
	DateTime? StartDate = null,
	DateTime? SchedualdDate = null,
	DateTime? DeadLineDate = null,
	DateTime? CompleteDate = null,
	string? Deliverables = null,
	string? Remarks = null,
	int? EngineerId = null
)
{
	public Task(int id = 0) : this(id, "", "", DateTime.Now) { }
	public Task(string alias, string description, int engineerId) 
		: this(0, alias, description, DateTime.Now, null, false, null, null, null, null, null, null, null, engineerId) { }
}
