namespace BO;

/// <summary> Milestone in task. </summary>
/// <param name="Id">Milestone's unique id</param>
/// <param name="Alias">Milestone's alias</param>
public class MilestoneInTask
{
	public int Id { init; get; }
	public required string Alias { set; get; }
}
