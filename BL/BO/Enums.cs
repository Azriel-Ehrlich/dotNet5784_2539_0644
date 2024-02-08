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
}

public enum ProjectStatus
{
	plan,
	schedule,
	execute
}

public enum TaskUpdate
{
	Alias = 1,
	Description,
	Engineer,
	Remraks,
	Deliverable,
	Dependencies,
	RequiredTimeEffort,
	Comlexity,
	StartDate,
	CompleteTask
}

public enum EngineerUpdate
{
	Email = 1,
	cost,
	Name,
	ExperienceLevel,
	Task
}

public enum EngineerExperienceWithAll
{
	Beginner,
	AdvancedBeginner,
	Intermediate,
	Advanced,
	Expert,
	All
}
