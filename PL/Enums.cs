using System.Collections;

namespace PL;


public enum EngineerExperienceWithAll
{
	Beginner,
	AdvancedBeginner,
	Intermediate,
	Advanced,
	Expert,
	All
}
public enum EngineerExperienceWithAllAndDeletedTask
{
	Beginner,
	AdvancedBeginner,
	Intermediate,
	Advanced,
	Expert,
	All,
	DeletedTask
}


internal class EngineerExpirieneCollection : IEnumerable
{
	static readonly IEnumerable<EngineerExperienceWithAll> s_enums = (Enum.GetValues(typeof(EngineerExperienceWithAll)) as IEnumerable<EngineerExperienceWithAll>)!;

	public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class EngineerExpirieneWithDeletedTaskCollection : IEnumerable
{
	static readonly IEnumerable<EngineerExperienceWithAllAndDeletedTask> s_enums = (Enum.GetValues(typeof(EngineerExperienceWithAllAndDeletedTask)) as IEnumerable<EngineerExperienceWithAllAndDeletedTask>)!;

	public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class EngineerExpirieneWithoutAllCollection : IEnumerable
{
	static readonly IEnumerable<BO.EngineerExperience> s_enums = (Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

	public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

/// <summary> class to store all the constant values </summary>
internal class ConstantValues
{
	public static readonly int NO_ID = -1;
	public static readonly int GANT_CHART_MAGIC_NUMBER = 10;
}
