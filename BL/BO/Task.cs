namespace BO;

/// <summary> This class represents task. </summary>
/// <param name="Id">Task's unique id</param>
/// <param name="Description">Task's description</param>
/// <param name="Alias">Task's alias</param>
/// <param name="CreatedAtDate">Date when the task was added to the system</param>
/// <param name="Status">Task's status</param>
/// <param name="Dependencies">relevant only before schedule is built</param>
/// <param name="Milestone">calculated when building schedule, populated if there is milestone in dependency, relevant only after schedule is built</param>
/// <param name="RequiredEffortTime">how many men-days needed for the task</param>
/// <param name="StartDate">the real start date</param>
/// <param name="ScheduledDate">the planned start date</param>
/// <param name="ForecastDate">calcualed planned completion date</param>
/// <param name="DeadlineDate">the latest complete date</param>
/// <param name="CompleteDate">real completion date</param>
/// <param name="Deliverables">description of deliverables for MS copmletion</param>
/// <param name="Remarks">free remarks from project meetings</param>
/// <param name="Engineer">the engineer assigned to the task</param>
/// <param name="Copmlexity">minimum expirience for engineer to assign</param>
public class Task
{
	public int Id { init; get; }
	public required string Alias { set; get; }
	public required string Description { set; get; }
	public DateTime CreatedAtDate { init; get; }
	public Status Status { set; get; }
	public List<TaskInList>? Dependencies { set; get; }
	public MilestoneInTask? Milestone { set; get; }
	public TimeSpan RequiredEffortTime { set; get; }
	public DateTime StartDate { set; get; }
	public DateTime ScheduledDate { set; get; }
	public DateTime ForecastDate { set; get; }
	public DateTime DeadlineDate { set; get; }
	public DateTime CompleteDate { set; get; }
	public string? Deliverables { set; get; }
	public string? Remarks { set; get; }
	public EngineerInTask? Engineer { set; get; }
	public EngineerExperience? Copmlexity { set; get; }
}
