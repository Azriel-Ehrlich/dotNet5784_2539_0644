namespace DalTest;

using DalApi;
using DO;
using System.Xml.Linq;

public static class Initialization
{
	private static ITask? s_dalTask;
	private static IEngineer? s_dalEngineer;
	private static IDependency? s_dalDependency;
	private static readonly Random s_rand = new();

	private static readonly string[] NAMES =
	{
		"Abraham", "Stewart", "Warren", "Anthony", "Alan",
		"Sean", "Jonathan", "Nathan", "Matt", "Stephen",
		"Christian", "David", "Brandon", "Max",  "Adrian",
		"Piers", "Lucas", "Robert", "Joseph", "Blake"
	}; // Thanks to AI for the list of names
	private static readonly int MIN_ID = 20000000, MAX_ID = 40000000;

	private static void createEngineers()
	{
		s_dalEngineer = new Dal.EngineerImplementation();

		int _id;
		foreach (var _name in NAMES)
		{
			do _id = s_rand.Next(MIN_ID, MAX_ID);
			while (s_dalEngineer.Read(_id) != null);

			Engineer newEng = new Engineer(_id,
				_name + "@eng.com", s_rand.Next(60, 200), _name,
				(EngineerExperience)s_rand.Next(Enum.GetNames(typeof(EngineerExperience)).Length)
			);

			s_dalEngineer.Create(newEng);
		}
	}


}
