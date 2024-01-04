namespace DalTest;

using DalApi;
using DO;

public static class Initialization
{
	private static ITask? s_dalTask;
	private static IEngineer? s_dalEngineer;
	private static IDependency? s_dalDependency;
	private static readonly Random s_rand = new();
	private static readonly int MIN_ID = 20000000, MAX_ID = 40000000;

	/*
	 * our project is to build a spaceship and found a colony of green aliens on Mars
	 */

	// TODO: Maor Noy said that we need to create DB arrays and them create objects inside the loops

	// our tasks:
	// TODO: insert normal dates and times
	private static readonly Task[] TASKS = {
		// preperations for the mission:
		new Task(0, "plane", "plane the mission", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),
	
		// recruit a team:
		new Task(0, "recruit 1", "recruit a team of engineers", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),
		new Task(0, "recruit 2", "recruit a team of physicists", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),
		new Task(0, "recruit 3", "recruit a team of astronauts", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),

		// research:
		new Task(0, "search 1", "Research strong and cheap materials to build the spacecraft", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),
		new Task(0, "search 2", "Find the best flight route", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),
		new Task(0, "search 3", "Make sure they don't collide with meteors", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),

		// build the spaceship:
		new Task(0, "Build 1", "Build a powerful engine", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),
		new Task(0, "Build 2", "build the body of the spaceship", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),

		// buy equipment:
		new Task(0, "equipment 1", "bring radios", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),
		new Task(0, "equipment 2", "fill food", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),
		new Task(0, "equipment 3", "Bring space suits", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),

		// training:
		new Task(0, "training 1", "train the crew to fly", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),
		new Task(0, "training 2", "Train the staff in the control room", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),
		new Task(0, "training 3", "Make sure they are in sync", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),

		// manning:
		new Task(0, "manning", "Man the spaceship", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),

		// preperations for launch:
		new Task(0, "Preparations 1", "Find a truck big enough", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),
		new Task(0, "Preparations 2", "Place the spaceship in place", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),
		new Task(0, "Preparations 3", "Fill up fuel", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),

		// launch:
		new Task(0, "Dispatch 1", "Dramatic countdown", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),
		new Task(0, "Dispatch 2", "Launch the spaceship", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),
		new Task(0, "Dispatch 3", "Thunderous applause", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),

		// space travel:
		new Task(0, "space travel", "Look for aliens", DateTime.Now, TimeSpan.FromHours(100), false, EngineerExperience.Beginner, null, new DateTime(2020, 1, 1), null, null, "the plane of the mission", null, 0),

		// To be continued...
	};

	// our amazing team:
	private static readonly Engineer[] ENGINEERS = {
		new Engineer(0, "alan@gmail.com", 80, "Alan", EngineerExperience.AdvancedBeginner), // engineer of building the spaceship
		new Engineer(0, "mic@gmail.com", 300, "Michael", EngineerExperience.Expert), // engineer of the flight route
		new Engineer(0, "brandon@gmail.com", 200, "Brandon", EngineerExperience.Advanced), // space explorer
		new Engineer(0, "nati@gmail.com", 150, "Natalie", EngineerExperience.Intermediate), // astronaut
		new Engineer(0, "elizabeth123@gmail.com", 60, "Elizabeth", EngineerExperience.Beginner) // logistics manager
	};

	// the dependencies between the tasks:
	private static readonly Dependency[] DEPENDENCIES = {
		// preperations for the mission: the mission can start only after the plane is ready
		new Dependency(0, 0, 0),

		// recruit a team: the team can start working only after the mission is planned
		new Dependency(0, 1, 0), // recruit engineers
		new Dependency(0, 2, 0), // recruit physicists
		new Dependency(0, 3, 0), // recruit astronauts

		// research: we need to know the materials before we can build the spaceship
		new Dependency(0, 4, 1), // after recruiting the engineers, we can start the research of materials
		new Dependency(0, 5, 2), // after recruiting the physicists, we can start the research of the flight route
		new Dependency(0, 6, 2), // after recruiting the physicists, we can start the research of the meteors

		// build the spaceship:
		new Dependency(0, 7, 4), // after researching the materials, we can start building the engine
		new Dependency(0, 8, 4), // after researching the materials, we can start building the spaceship

		// buy equipment:
		new Dependency(0, 9, 0), // don't need nothing special to buy the radios
		new Dependency(0, 10, 0), // we need to buy food for the astronauts
		new Dependency(0, 11, 4), // we need create the space suits especially for the astronauts

		// training:
		new Dependency(0, 12, 11), // after buying the radios, we can start training the crew
		new Dependency(0, 13, 11), // after buying the radios, we can start training the control room
		new Dependency(0, 14, 13), // after training the control room, we can check if they are in sync

		// manning:
		new Dependency(0, 15, 12), // after training we can manning the spaceship

		// preperations for launch:
		new Dependency(0, 16, 8), // after building the spaceship, we can find a truck big enough
		new Dependency(0, 17, 16), // after finding a truck big enough, we can place the spaceship in place
		new Dependency(0, 18, 17), // after placing the spaceship in place, we can fill up fuel

		// launch:
		new Dependency(0, 19, 18), // after filling up fuel, we can start the countdown
		new Dependency(0, 20, 19), // after the countdown, we can launch the spaceship
		new Dependency(0, 21, 20), // after launching the spaceship, we can hear the applause

		// space travel:
		new Dependency(0, 22, 21) // after launching the spaceship, we can start looking for aliens
	};

	/// <summary>
	/// Create the list of the engineers
	/// </summary>
	private static void createEngineers()
	{
		int id;
		foreach (var eng in ENGINEERS)
		{
			do id = s_rand.Next(MIN_ID, MAX_ID + 1);
			while (s_dalEngineer!.Read(id) != null);
			s_dalEngineer.Create(eng with { Id = id });
		}
	}

	/// <summary>
	/// Create the list of the tasks
	/// </summary>
	private static void createTasks()
	{
		foreach (var task in TASKS)
		{
			s_dalTask!.Create(task);
		}
	}

	/// <summary>
	/// Create the list of the dependencies between the tasks
	/// </summary>
	private static void createDependencies()
	{
		foreach (var dep in DEPENDENCIES)
		{
			s_dalDependency!.Create(dep);
		}
	}

	/// <summary>
	/// Initialize the DAL
	/// </summary>
	/// <param name="dalTask">Implementation of <see cref="ITask"/> </param>
	/// <param name="dalEngineer">Implementation of <see cref="IEngineer"/> </param>
	/// <param name="dalDependency">Implementation of <see cref="IDependency"/> </param>
	/// <exception cref="NullReferenceException">Thrown when one of the parameters is null</exception>
	public static void Do(ITask? dalTask, IEngineer? dalEngineer, IDependency? dalDependency)
	{
		if (dalTask == null || dalEngineer == null || dalDependency == null)
			throw new NullReferenceException("DAL can not be null!");

		s_dalTask = dalTask;
		s_dalEngineer = dalEngineer;
		s_dalDependency = dalDependency;

		createEngineers();
		createTasks();
		createDependencies();
	}
}
