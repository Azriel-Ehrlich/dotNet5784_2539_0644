namespace DO;

/// <summary> A task to be done by an engineer </summary>
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
	DateTime? ScheduledDate = null,
	DateTime? DeadLineDate = null,
	DateTime? CompleteDate = null,
	string? Deliverables = null,
	string? Remarks = null,
	int? EngineerId = null,
	bool Active = true
)
{
	public Task() : this(0, "", "", DateTime.Now) { }
	public Task(string alias, string description, int engineerId) 
		: this(0, alias, description, DateTime.Now, null, false, null, null, null, null, null, null, null, engineerId) { }

    // This method controls whether the RequiredEffortTime property should be serialized
    public bool ShouldSerializeRequiredEffortTime()
    {
        return RequiredEffortTime.HasValue;
    }

    // This method controls whether the StartDate property should be serialized
    public bool ShouldSerializeStartDate()
    {
        return StartDate.HasValue;
    }

    // This method controls whether the SchedualdDate property should be serialized
    public bool ShouldSerializeSchedualdDate()
    {
        return ScheduledDate.HasValue;
    }

    // This method controls whether the DeadLineDate property should be serialized
    public bool ShouldSerializeDeadLineDate()
    {
        return DeadLineDate.HasValue;
    }

    // This method controls whether the CompleteDate property should be serialized
    public bool ShouldSerializeCompleteDate()
    {
        return CompleteDate.HasValue;
    }

    // This method controls whether the Deliverables property should be serialized
    public bool ShouldSerializeDeliverables()
    {
        return !string.IsNullOrEmpty(Deliverables);
    }

    // This method controls whether the Remarks property should be serialized
    public bool ShouldSerializeRemarks()
    {
        return !string.IsNullOrEmpty(Remarks);
    }

    // This method controls whether the EngineerId property should be serialized
    public bool ShouldSerializeEngineerId()
    {
        return EngineerId.HasValue;
    }
}
