using System.Collections;

namespace PL;


public enum EngineerExperienceWithAllAndDeleted
{
	Beginner,
	AdvancedBeginner,
	Intermediate,
	Advanced,
	Expert,
	All,
	Deleted
}


internal class EngineerExpirieneWithAllCollection : IEnumerable
{
	static readonly IEnumerable<EngineerExperienceWithAllAndDeleted> s_enums = (Enum.GetValues(typeof(EngineerExperienceWithAllAndDeleted)) as IEnumerable<EngineerExperienceWithAllAndDeleted>)!;

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
	public static readonly int GANTT_CHART_MAGIC_NUMBER = 10;
}
