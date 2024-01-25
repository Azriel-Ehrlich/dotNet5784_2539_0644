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
}
