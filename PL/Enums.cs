using System.Collections;

namespace PL;

internal class EngineerExpirieneCollection : IEnumerable
{
	static readonly IEnumerable<BO.EngineerExperienceWithAll> s_enums = (Enum.GetValues(typeof(BO.EngineerExperienceWithAll)) as IEnumerable<BO.EngineerExperienceWithAll>)!;

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
