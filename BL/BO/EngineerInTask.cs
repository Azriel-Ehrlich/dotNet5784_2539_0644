using BlImplementation;

namespace BO;

/// <summary> Engineer in task. </summary>
/// <param name="Id">Engineer's unique id</param>
/// <param name="Name">Engineer's full-name</param>
public class EngineerInTask
{
	public int Id { init; get; }
	public required string Name { set; get; }


	public override string ToString() => this.ToStringProperty();
}
