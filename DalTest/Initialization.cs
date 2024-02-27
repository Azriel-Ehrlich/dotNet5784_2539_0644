namespace DalTest;

using Dal;
using DalApi;
using DO;

public static class Initialization
{
    private static IDal? s_dal;
    private static readonly Random s_rand = new();
    private static readonly int MIN_ID = 200000000, MAX_ID = 400000000;

	/*
	 * our project is to build a spaceship and conduct a reasearch on Mars
	 */

	// For now we save in tuples the data we want to insert into the database.
	// In the future we will probably read the data from a file.

	// our tasks:
	private static readonly Task[] TASKS = {
		// Initiating the mission planning process
		new Task(0, "Mission Planning", "Plan the mission to Mars", DateTime.Now.AddYears(-4), TimeSpan.FromDays(365 * 3), false, EngineerExperience.Expert, null, null, null, null, "Mission plan details", null, null),
		
		// Choosing the crew for the mission
		new Task(0, "Crew Selection", "Select a qualified crew for the mission", DateTime.Now.AddYears(-2), TimeSpan.FromDays(365 * 2), false, EngineerExperience.Expert, null, null, null, null, "Crew selection criteria", null, null),
		
		// Creating the design for the spacecraft
		new Task(0, "Spacecraft Design", "Design the spacecraft for the mission", DateTime.Now.AddYears(-1), TimeSpan.FromDays(365), false, EngineerExperience.Intermediate, null, null, null, null, "Spacecraft design specifications", null, null),
		
		// Working on the launch system
		new Task(0, "Launch System Development", "Develop the launch system for the spacecraft", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Beginner, null, null, null, null, "Launch system development plan", null, null),
		
		// Planning for orbital insertion
		new Task(0, "Orbital Insertion Planning", "Plan the orbital insertion phase of the mission", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Expert, null, null, null, null, "Orbital insertion plan", null,null),
		
		// Designing the habitat for Mars surface
		new Task(0, "Surface Habitat Design", "Design the habitat for the Mars surface", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Intermediate, null, null, null, null, "Surface habitat design details", null, null),
		
		// Planning for mission supplies and resources
		new Task(0, "Supply and Resource Planning", "Plan the supply and resource management for the mission", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Beginner, null, null, null, null, "Supply and resource planning details", null, null),
		
		// Creating communication systems
		new Task(0, "Communication Systems", "Develop communication systems for the mission", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Expert, null, null, null, null, "Communication systems specifications", null, null),
		
		// Planning for Mars entry and landing
		new Task(0, "Mars Entry and Landing", "Plan the entry and landing on Mars", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Intermediate, null, null, null, null, "Mars entry and landing plan", null, null),
		
		// Creating equipment for surface exploration
		new Task(0, "Surface Exploration Equipment", "Develop equipment for surface exploration", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Beginner, null, null, null, null, "Surface exploration equipment details", null, null),
		
		// Planning for environmental monitoring
		new Task(0, "Environmental Monitoring", "Plan environmental monitoring for the mission", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Expert, null, null, null, null, "Environmental monitoring plan", null, null),
		
		// Creating emergency response plans
		new Task(0, "Emergency Response Planning", "Develop plans for emergency responses", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Intermediate, null, null, null, null, "Emergency response planning details", null, null),
		
		// Planning for crew health and medical support
		new Task(0, "Crew Health and Medical Planning", "Plan for crew health and medical needs", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Beginner, null, null, null, null, "Crew health and medical planning details", null, null),
		
		// Creating ascent vehicle design
		new Task(0, "Mars Ascent Vehicle Design", "Design the ascent vehicle for returning from Mars", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Expert, null, null, null, null, "Mars ascent vehicle design details", null, null),
		
		// Planning for the return journey
		new Task(0, "Return Journey Planning", "Plan the journey back to Earth", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Intermediate, null, null, null, null, "Return journey planning details", null, null),
		
		// Collecting and analyzing samples
		new Task(0, "Sample Collection and Analysis", "Collect and analyze surface samples", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Beginner, null, null, null, null, "Sample collection and analysis details", null, null),
		
		// Planning for crew well-being
		new Task(0, "Crew Exercise and Well-being", "Plan for crew exercise and well-being", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Expert, null, null, null, null, "Crew exercise and well-being plan", null, null),
		
		// Establishing communication with Earth
		new Task(0, "Communication with Earth", "Establish communication with Earth", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Intermediate, null, null	, null, null, "Communication with Earth plan", null, null),
		
		// Engaging with the public and outreach
		new Task(0, "Public Relations and Outreach", "Engage in public relations and outreach activities", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Beginner, null, null, null, null, "Public relations and outreach plan", null, null),
		
		// Analyzing mission outcomes
		new Task(0, "Post-Mission Analysis", "Conduct post-mission analysis", DateTime.Now, TimeSpan.FromDays(365), false, EngineerExperience.Expert, null, null, null, null, "Post-mission analysis details", null, null)
    };

    // our amazing team:
    private static readonly Engineer[] ENGINEERS = {
        new Engineer(385168384, "alan@gmail.com", 80, "Alan", EngineerExperience.AdvancedBeginner), // engineer of building the spaceship
		new Engineer(234566019, "mic@gmail.com", 300, "Michael", EngineerExperience.Expert), // engineer of the flight route
		new Engineer(220359006, "brandon@gmail.com", 200, "Brandon", EngineerExperience.Advanced), // space explorer
		new Engineer(272137229, "nati@gmail.com", 150, "Natalie", EngineerExperience.Intermediate), // astronaut
		new Engineer(371400813, "elizabeth123@gmail.com", 60, "Elizabeth", EngineerExperience.Beginner) // logistics manager
	};

    // the dependencies between the tasks:
    private static readonly Dependency[] DEPENDENCIES = {
        new Dependency(1, 0),   // Crew Selection depends on Mission Planning
		new Dependency(2, 1),   // Spacecraft Design depends on Crew Selection
		new Dependency(3, 2),   // Launch System Development depends on Spacecraft Design
		new Dependency(4, 3),   // Orbital Insertion Planning depends on Launch System Development
		new Dependency(5, 4),   // Surface Habitat Design depends on Orbital Insertion Planning
		new Dependency(6, 5),   // Supply and Resource Planning depends on Surface Habitat Design
		new Dependency(7, 6),   // Communication Systems depends on Supply and Resource Planning
		new Dependency(8, 7),   // Mars Entry and Landing depends on Communication Systems
		new Dependency(9, 8),   // Surface Exploration Equipment depends on Mars Entry and Landing
		new Dependency(10, 9),  // Environmental Monitoring depends on Surface Exploration Equipment
		new Dependency(11, 10), // Emergency Response Planning depends on Environmental Monitoring
		new Dependency(12, 11), // Crew Health and Medical Planning depends on Emergency Response Planning
		new Dependency(13, 12), // Mars Ascent Vehicle Design depends on Crew Health and Medical Planning
		new Dependency(14, 13), // Return Journey Planning depends on Mars Ascent Vehicle Design
		new Dependency(15, 14), // Sample Collection and Analysis depends on Return Journey Planning
		new Dependency(16, 15), // Crew Exercise and Well-being depends on Sample Collection and Analysis
		new Dependency(17, 16), // Communication with Earth depends on Crew Exercise and Well-being
		new Dependency(18, 17), // Public Relations and Outreach depends on Communication with Earth
		new Dependency(19, 18), // Post-Mission Analysis depends on Public Relations and Outreach
		new Dependency(10, 5),  // Emergency Response Planning depends on Surface Habitat Design
		new Dependency(5, 2),  // Surface Habitat Design depends on Spacecraft Design
	};

    /// <summary> Create the list of the engineers </summary>
    private static void createEngineers()
    {
        int id;
        foreach (var eng in ENGINEERS)
        {
			/*
            do id = s_rand.Next(MIN_ID, MAX_ID + 1);
            while (s_dal!.Engineer.Read(id) != null);
            s_dal.Engineer.Create(eng with { Id = id });
			*/

			s_dal!.Engineer.Create(eng); // do not use random id. its annoying when AH is debugging.
        }
    }

    /// <summary> Create the list of the tasks </summary>
    private static void createTasks()
    {
        foreach (var task in TASKS)
        {
            s_dal!.Task.Create(task);
        }
    }

    /// <summary> Create the list of the dependencies between the tasks </summary>
    private static void createDependencies()
    {
        foreach (var dep in DEPENDENCIES)
        {
            s_dal!.Dependency.Create(dep);
        }
    }

    /// <summary> Initialize the DAL </summary>
    /// <exception cref="NullReferenceException">Thrown when one of the parameters is null</exception>
    public static void Do()
    {
        s_dal = DalApi.Factory.Get;

		if (s_dal is null)
			throw new NullReferenceException("DAL object can not be null!");

		// when we use new Dal, we want to reset the files:
		s_dal.Reset();

        createEngineers();
        createTasks();
        createDependencies();
    }

	/// <summary> Reset the DAL </summary>
	public static void Reset()
	{
		s_dal = DalApi.Factory.Get;
		s_dal.Reset();
	}
}
