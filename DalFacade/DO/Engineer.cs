namespace DO;

/// <summary>
/// Basic information about an engineer
/// </summary>
/// <param name="Id">Engineer's unique id</param>
/// <param name="Email">Engineer's email</param>
/// <param name="Cost">Cost per hour of the engineer</param>
/// <param name="Name">Engineer's full-name</param>
/// <param name="Level">Engineer's Experience</param>
/// <param name="Active">Engineer's state</param>
public record Engineer
(
	int Id,
	string Email,
	double Cost,
	string Name,
	EngineerExperience Level,
	bool Active = true
)
{
	public Engineer(int id = 0) : this(id, "", 0, "", EngineerExperience.Beginner) { }
}
