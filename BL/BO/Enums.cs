namespace BO;

public enum EngineerExperience
{
	Beginner,
	AdvancedBeginner,
	Intermediate,
	Advanced,
	Expert
}

public enum Status
{
	Unscheduled,
	Scheduled,
	OnTrack,
	Done
}

public enum MainChoices
{
	Exit,
	Task,
	Engineer,
	Schedule
}

public enum SubMenuChoices
{
	Exit,
	Create,
	Read,
	ReadAll,
	Update,
	Delete,
	UpdateScheduledDate // used only for tasks
}
