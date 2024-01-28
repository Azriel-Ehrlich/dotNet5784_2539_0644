using BlImplementation;

namespace BO;

/// <summary> Task in engineer. </summary>
/// <param name="Id">Task's unique id</param>
/// <param name="Alias">Task's alias</param>
public class TaskInEngineer
{
	public int Id { init; get; }
	public required string Alias { set; get; }


	public override string ToString() => this.ToStringProperty();
}
