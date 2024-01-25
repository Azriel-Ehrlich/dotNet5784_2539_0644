namespace BlApi;

internal interface IBl
{
	public IEngineer Engineer { get; }
	public ITask Task { get; }
	//public IMileStone MileStone { get; } // TODO: uncomment when we have milestones
}
