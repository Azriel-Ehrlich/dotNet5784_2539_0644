using BlImplementation;


namespace BO;

/// <summary> Task in list. </summary>
/// <param name="Id">Task's unique id</param>
/// <param name="Description">Task's description</param>
/// <param name="Alias">Task's alias</param>
/// <param name="Status">Task's status</param>
public class TaskInList
{
	public int Id { init; get; }
	public required string Description { set; get; }
	public required string Alias { set; get; }
	public Status Status { set; get; }


	public override string ToString() => this.ToStringProperty();

    /// <summary> Cast a BO.Task to a BO.TaskInList. </summary>
    /// <param name="task"> The BO.Task to cast. </param>
    /// <returns> The BO.TaskInList. </returns>
    public static BO.TaskInList FromTask(BO.Task task)
    {
        return new BO.TaskInList()
        {
            Id = task.Id,
            Alias = task.Alias,
            Description = task.Description,
            Status = task.Status
        };
    }
}
