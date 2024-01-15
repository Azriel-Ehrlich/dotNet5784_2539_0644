namespace DalTest;

using DalApi;
using DO;

public static class Initialization
{
	private static IDal? s_dal;
	private static readonly Random s_rand = new();
	private static readonly int MIN_ID = 20000000, MAX_ID = 40000000;

	// TODO: create normal arrays of classes instaces and not tuples

	/*
	 * our project is to build a spaceship and conduct a reasearch on Mars
	 */

	// For now we save in tuples the data we want to insert into the database.
	// In the future we will probably read the data from a file.

	// our tasks:
	private static readonly (string, string, DateTime, TimeSpan?,
		bool, EngineerExperience?, DateTime?, DateTime?, DateTime?,
		DateTime?, string?, string?, int?)[] TASKS = {
		// Initiating the mission planning process
		("Mission Planning", "Plan the mission to Mars", DateTime.Now.AddYears(-3), TimeSpan.FromDays(365 * 3), false, EngineerExperience.Expert, null, new DateTime(2027, 1, 1), null, null, "Mission plan details", null, 0),
		
		// Choosing the crew for the mission
		("Crew Selection", "Select a qualified crew for the mission", DateTime.Now.AddYears(-2), TimeSpan.FromDays(365 * 2), false, EngineerExperience.Expert, null, new DateTime(2028, 1, 1), null, null, "Crew selection criteria", null, 0),
		
		// Creating the design for the spacecraft
		("Spacecraft Design", "Design the spacecraft for the mission", DateTime.Now.AddYears(-1), TimeSpan.FromDays(365), false, EngineerExperience.Intermediate, null, new DateTime(2029, 1, 1), null, null, "Spacecraft design specifications", null, 0),
		
		// Working on the launch system
		("Launch System Development", "Develop the launch system for the spacecraft", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Beginner, null, new DateTime(2030, 1, 1), null, null, "Launch system development plan", null, 0),
		
		// Planning for orbital insertion
		("Orbital Insertion Planning", "Plan the orbital insertion phase of the mission", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Expert, null, new DateTime(2031, 1, 1), null, null, "Orbital insertion plan", null, 0),
		
		// Designing the habitat for Mars surface
		("Surface Habitat Design", "Design the habitat for the Mars surface", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Intermediate, null, new DateTime(2032, 1, 1), null, null, "Surface habitat design details", null, 0),
		
		// Planning for mission supplies and resources
		("Supply and Resource Planning", "Plan the supply and resource management for the mission", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Beginner, null, new DateTime(2033, 1, 1), null, null, "Supply and resource planning details", null, 0),
		
		// Creating communication systems
		("Communication Systems", "Develop communication systems for the mission", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Expert, null, new DateTime(2034, 1, 1), null, null, "Communication systems specifications", null, 0),
		
		// Planning for Mars entry and landing
		("Mars Entry and Landing", "Plan the entry and landing on Mars", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Intermediate, null, new DateTime(2035, 1, 1), null, null, "Mars entry and landing plan", null, 0),
		
		// Creating equipment for surface exploration
		("Surface Exploration Equipment", "Develop equipment for surface exploration", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Beginner, null, new DateTime(2036, 1, 1), null, null, "Surface exploration equipment details", null, 0),
		
		// Planning for environmental monitoring
		("Environmental Monitoring", "Plan environmental monitoring for the mission", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Expert, null, new DateTime(2037, 1, 1), null, null, "Environmental monitoring plan", null, 0),
		
		// Creating emergency response plans
		("Emergency Response Planning", "Develop plans for emergency responses", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Intermediate, null, new DateTime(2038, 1, 1), null, null, "Emergency response planning details", null, 0),
		
		// Planning for crew health and medical support
		("Crew Health and Medical Planning", "Plan for crew health and medical needs", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Beginner, null, new DateTime(2039, 1, 1), null, null, "Crew health and medical planning details", null, 0),
		
		// Creating ascent vehicle design
		("Mars Ascent Vehicle Design", "Design the ascent vehicle for returning from Mars", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Expert, null, new DateTime(2040, 1, 1), null, null, "Mars ascent vehicle design details", null, 0),
		
		// Planning for the return journey
		("Return Journey Planning", "Plan the journey back to Earth", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Intermediate, null, new DateTime(2041, 1, 1), null, null, "Return journey planning details", null, 0),
		
		// Collecting and analyzing samples
		("Sample Collection and Analysis", "Collect and analyze surface samples", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Beginner, null, new DateTime(2042, 1, 1), null, null, "Sample collection and analysis details", null, 0),
		
		// Planning for crew well-being
		("Crew Exercise and Well-being", "Plan for crew exercise and well-being", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Expert, null, new DateTime(2043, 1, 1), null, null, "Crew exercise and well-being plan", null, 0),
		
		// Establishing communication with Earth
		("Communication with Earth", "Establish communication with Earth", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Intermediate, null, new DateTime(2044, 1, 1), null, null, "Communication with Earth plan", null, 0),
		
		// Engaging with the public and outreach
		("Public Relations and Outreach", "Engage in public relations and outreach activities", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Beginner, null, new DateTime(2045, 1, 1), null, null, "Public relations and outreach plan", null, 0),
		
		// Analyzing mission outcomes
		("Post-Mission Analysis", "Conduct post-mission analysis", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Expert, null, new DateTime(2046, 1, 1), null, null, "Post-mission analysis details", null, 0)
	};

	// our amazing team:
	private static readonly (string, double, string, EngineerExperience)[] ENGINEERS = {
		("alan@gmail.com", 80, "Alan", EngineerExperience.AdvancedBeginner), // engineer of building the spaceship
		("mic@gmail.com", 300, "Michael", EngineerExperience.Expert), // engineer of the flight route
		("brandon@gmail.com", 200, "Brandon", EngineerExperience.Advanced), // space explorer
		("nati@gmail.com", 150, "Natalie", EngineerExperience.Intermediate), // astronaut
		("elizabeth123@gmail.com", 60, "Elizabeth", EngineerExperience.Beginner) // logistics manager
	};

	// the dependencies between the tasks:
	private static readonly (int, int)[] DEPENDENCIES = {
		(1, 0),   // Crew Selection depends on Mission Planning
		(2, 1),   // Spacecraft Design depends on Crew Selection
		(3, 2),   // Launch System Development depends on Spacecraft Design
		(4, 3),   // Orbital Insertion Planning depends on Launch System Development
		(5, 4),   // Surface Habitat Design depends on Orbital Insertion Planning
		(6, 5),   // Supply and Resource Planning depends on Surface Habitat Design
		(7, 6),   // Communication Systems depends on Supply and Resource Planning
		(8, 7),   // Mars Entry and Landing depends on Communication Systems
		(9, 8),   // Surface Exploration Equipment depends on Mars Entry and Landing
		(10, 9),  // Environmental Monitoring depends on Surface Exploration Equipment
		(11, 10), // Emergency Response Planning depends on Environmental Monitoring
		(12, 11), // Crew Health and Medical Planning depends on Emergency Response Planning
		(13, 12), // Mars Ascent Vehicle Design depends on Crew Health and Medical Planning
		(14, 13), // Return Journey Planning depends on Mars Ascent Vehicle Design
		(15, 14), // Sample Collection and Analysis depends on Return Journey Planning
		(16, 15), // Crew Exercise and Well-being depends on Sample Collection and Analysis
		(17, 16), // Communication with Earth depends on Crew Exercise and Well-being
		(18, 17), // Public Relations and Outreach depends on Communication with Earth
		(19, 18), // Post-Mission Analysis depends on Public Relations and Outreach
		(10, 5),  // Emergency Response Planning depends on Surface Habitat Design
		(11, 10), // Crew Health and Medical Planning depends on Emergency Response Planning
		(12, 11), // Mars Ascent Vehicle Design depends on Crew Health and Medical Planning
		(13, 12), // Return Journey Planning depends on Mars Ascent Vehicle Design
		(14, 13), // Sample Collection and Analysis depends on Return Journey Planning
		(15, 14), // Crew Exercise and Well-being depends on Sample Collection and Analysis
		(16, 15), // Communication with Earth depends on Crew Exercise and Well-being
		(17, 16), // Public Relations and Outreach depends on Communication with Earth
		(18, 17), // Post-Mission Analysis depends on Public Relations and Outreach
		(5, 2),  // Surface Habitat Design depends on Spacecraft Design
		(6, 5),  // Supply and Resource Planning depends on Surface Habitat Design
		(7, 6),  // Communication Systems depends on Supply and Resource Planning
		(8, 7),  // Mars Entry and Landing depends on Communication Systems
		(9, 8),  // Surface Exploration Equipment depends on Mars Entry and Landing
		(10, 9), // Environmental Monitoring depends on Surface Exploration Equipment
		(11, 10),// Emergency Response Planning depends on Environmental Monitoring
		(12, 11),// Crew Health and Medical Planning depends on Emergency Response Planning
		(13, 12),// Mars Ascent Vehicle Design depends on Crew Health and Medical Planning
		(14, 13) // Return Journey Planning depends on Mars Ascent Vehicle Design
	};

	/// <summary> Create the list of the engineers </summary>
	private static void createEngineers()
	{
		int id;
		foreach (var eng in ENGINEERS)
		{
			do id = s_rand.Next(MIN_ID, MAX_ID + 1);
			while (s_dal!.Engineer.Read(id) != null);
			s_dal.Engineer.Create(new Engineer(id, eng.Item1, eng.Item2, eng.Item3, eng.Item4));
		}
	}

	/// <summary> Create the list of the tasks </summary>
	private static void createTasks()
	{
		foreach (var task in TASKS)
		{
			s_dal!.Task.Create(new Task(
				0, task.Item1, task.Item2,
				task.Item3, task.Item4, task.Item5,
				task.Item6, task.Item7, task.Item8,
				task.Item9, task.Item10, task.Item11,
				task.Item12, task.Item13
			));
		}
	}

	/// <summary> Create the list of the dependencies between the tasks </summary>
	private static void createDependencies()
	{
		foreach (var (taskId, dependencyId) in DEPENDENCIES)
		{
			s_dal!.Dependency.Create(new Dependency(taskId, dependencyId));
		}
	}

	/// <summary> Initialize the DAL </summary>
	/// <param name="dalTask">Implementation of <see cref="ITask"/> </param>
	/// <param name="dalEngineer">Implementation of <see cref="IEngineer"/> </param>
	/// <param name="dalDependency">Implementation of <see cref="IDependency"/> </param>
	/// <exception cref="NullReferenceException">Thrown when one of the parameters is null</exception>
	public static void Do(IDal? dal)
	{
		s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!");

		createEngineers();
		createTasks();
		createDependencies();
	}
}
