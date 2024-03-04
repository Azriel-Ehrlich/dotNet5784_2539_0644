using BlImplementation;

namespace BO;

/// <summary> This class represents an engineer. </summary>
/// <param name="Id">Engineer's unique id</param>
/// <param name="Email">Engineer's email</param>
/// <param name="Cost">Cost per hour of the engineer</param>
/// <param name="Name">Engineer's full-name</param>
/// <param name="Level">Engineer's Experience</param>
/// <param name="Active">Engineer's state</param>
/// param  name="Task">Engineer's current task</param>
/// </summary>
public class Engineer
{
	public int Id { set; get; }
	public required string Email { set; get; }
	public double Cost { set; get; }
	public required string Name { set; get; }
	public EngineerExperience Level { set; get; }
	public bool IsActive { set; get; }
	public BO.TaskInEngineer? Task { set; get; }


	public override string ToString() => this.ToStringProperty();
}
