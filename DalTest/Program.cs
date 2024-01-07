namespace DalTest;

using Dal;
using DalApi;
using DO;

internal class Program
{
	private static IEngineer? s_dalEngineer = new EngineerImplementation();
	private static IDependency? s_dalDependency = new DependencyImplementation();
	private static ITask? s_dalTask = new TaskImplementation();


	/// <summary> The main function of the program. </summary>
	/// <param name="args"> The arguments of the program. </param>
	static void Main(string[] args)
	{
		Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependency); // initialize the data

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine("Welcome to the missions managing manu");
		Console.WriteLine("Our mission is to send a spaceship to space");
		Console.WriteLine("--------------------------------------------");

		// Dictionary of functions to call for each menu choice with cool syntax:
		Dictionary<MainChoices, Dictionary<CrudChoices, Action>> funcs = new()
		{
			{
				MainChoices.Engineer,
				new Dictionary<CrudChoices, Action>()
				{
					{ CrudChoices.Create, createNewEngineer },
					{ CrudChoices.Read, readEngineer },
					{ CrudChoices.ReadAll, readAllEngineers },
					{ CrudChoices.Update, updateEngineer },
					{ CrudChoices.Delete, deleteEngineer }
				}
			},
			{
				MainChoices.Task,
				new Dictionary<CrudChoices, Action>()
				{
					{ CrudChoices.Create, createNewTask },
					{ CrudChoices.Read, readTask },
					{ CrudChoices.ReadAll, readAllTasks },
					{ CrudChoices.Update, updateTask },
					{ CrudChoices.Delete, deleteTask }
				}
			},
			{
				MainChoices.Dependency,
				new Dictionary<CrudChoices, Action>()
				{
					{ CrudChoices.Create, createNewDependency },
					{ CrudChoices.Read, readDependency },
					{ CrudChoices.ReadAll, readAllDependencies },
					{ CrudChoices.Update, updateDependency },
					{ CrudChoices.Delete, deleteDependency }
				}
			}
		};

		MainChoices mainChoice = MainChoices.Engineer; // default value so the loop will start
		while (mainChoice != MainChoices.Exit)
		{
			try
			{
				mainChoice = getMainMenuChoice();
				if (mainChoice == MainChoices.Exit)
					break;

				CrudChoices crudChoice = CrudChoices.Create; // default value so the loop will start
				while (crudChoice != CrudChoices.Exit)
				{
					try
					{
						crudChoice = getCrudMenuChoice(mainChoice);
						if (crudChoice == CrudChoices.Exit)
							break;

						Console.ForegroundColor = ConsoleColor.White;

						funcs[mainChoice][crudChoice](); // call the function with cool syntax :)
					}
					catch (Exception exp)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Error: " + exp.Message);
					}
				}
			}
			catch (Exception exp)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Error: " + exp.Message);
			}
		}

		Console.ForegroundColor = ConsoleColor.White; // reset the color
	}


	/*
	 * Helper functions:
	 */

	/// <summary> Reads an integer from the user. </summary>
	/// <returns> The integer that the user entered. </returns>
	/// <exception cref="Exception"> Thrown when the user entered an invalid input. </exception>
	static int readInt()
	{
		int res;
		if (!int.TryParse(Console.ReadLine(), out res))
			throw new Exception("invalid input");
		return res;
	}

	/// <summary> Reads a double from the user. </summary>
	/// <returns> The double that the user entered. </returns>
	/// <exception cref="Exception"> Thrown when the user entered an invalid input. </exception>
	static double readDouble()
	{
		double res;
		if (!double.TryParse(Console.ReadLine(), out res))
			throw new Exception("invalid input please try again");

		return res;
	}

	/*
	 * Menu functions:
	 */

	/// <summary> Gets the choice of the user for the main menu. </summary>
	/// <returns> The choice of the user for the main menu. </returns>
	static MainChoices getMainMenuChoice()
	{
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine("Main manu:");
		Console.WriteLine("Please choose one of the following options:");
		Console.WriteLine("1) open the Engineer managing manu");
		Console.WriteLine("2) open the Task managing manu");
		Console.WriteLine("3) open the Dependency managing manu");
		Console.WriteLine("0) exit");

		MainChoices choose = (MainChoices)readInt();
		if (choose < MainChoices.Exit || choose > MainChoices.Dependency)
			throw new Exception("invalid input please try again");
		return choose;
	}

	/// <summary> Gets the choice of the user for the CRUD menu. </summary>
	/// <param name="mainChoice"> The choice of the user for the main menu. used to print the correct menu. </param>
	/// <returns> The choice of the user for the CRUD menu. </returns>
	static CrudChoices getCrudMenuChoice(MainChoices mainChoice)
	{
		Console.ForegroundColor = ConsoleColor.Yellow;
		// print message about the current menu:
		Console.WriteLine($"{mainChoice} managing manu:");
		Console.WriteLine("Please choose one of the requested actions:");
		Console.WriteLine("0) return to the main manu");
		Console.WriteLine("1) create new item");
		Console.WriteLine("2) read a specific item");
		Console.WriteLine("3) read all of the items");
		Console.WriteLine("4) update a specific item");
		Console.WriteLine("5) delete a specific item");

		CrudChoices choose = (CrudChoices)readInt();
		if (choose < CrudChoices.Exit || choose > CrudChoices.Delete)
			throw new Exception("invalid input please try again");
		return choose;
	}


	/*
	 * Task functions
	 */

	/// <summary> Gets a task from the user. </summary>
	/// <returns> The task that the user entered. </returns>
	static Task getTaskFromUser()
	{
		Console.Write("enter the name of the task: ");
		string alias = Console.ReadLine()!;

		Console.Write("enter the description of the task: ");
		string description = Console.ReadLine()!;

		Console.Write("what is the id of the Engineer who assigned to do the Task: ");
		int engineerId = readInt();
		Console.WriteLine("enter start date");

		string? line;

		line = Console.ReadLine();
		DateTime? startDate = null, deadLine = null;
		if (line != null && line != "")
		{
			startDate = DateTime.Parse(line);
		}

		Console.WriteLine("enter deadLine");
		line = Console.ReadLine();
		if (line != null && line != "")
		{
			deadLine = DateTime.Parse(line);
		}

		TimeSpan? requiredEffortTime = deadLine - startDate;
		Console.Write("is this task is a milestone? ");
		bool isMilestone = bool.Parse(Console.ReadLine()!);

		Console.Write("enter the complexity of the task (0-4): ");
		EngineerExperience complexity = (EngineerExperience)readInt();

		return new Task(0, alias, description, DateTime.Now,
			requiredEffortTime, isMilestone, complexity, startDate,
			null, deadLine, null, "", "", engineerId);
	}
	/// <summary> Creates a new task and adds it to the database. </summary>
	static void createNewTask()
	{
		s_dalTask!.Create(getTaskFromUser());
	}
	/// <summary> Reads a task from the database and prints it to the console. </summary>
	static void readTask()
	{
		Console.Write("enter the id of the Task: ");
		int id = readInt();

		Task? task = s_dalTask!.Read(id);
		if (task == null)
			Console.WriteLine("the Task does not exist");
		else
			Console.WriteLine(task);
	}
	/// <summary> Reads all tasks from the database and prints them to the console. </summary>
	static void readAllTasks()
	{
		foreach (var task in s_dalTask!.ReadAll())
			Console.WriteLine($"> {task}");
	}
	/// <summary> Updates a task in the database. </summary>
	static void updateTask()
	{
		Console.Write("enter the id of the Task: ");
		int id = readInt();
		Task task = getTaskFromUser();
		s_dalTask!.Update(task with { Id = id });
	}
	/// <summary> Deletes a task from the database. </summary>
	static void deleteTask()
	{
		Console.Write("enter the id of the Task: ");
		int id = readInt();
		s_dalTask!.Delete(id);
	}


	/*
	 * Engineer functions
	 */

	/// <summary> Gets an engineer from the user. </summary>
	/// <returns> The engineer that the user entered. </returns>
	static Engineer getEngineerFromUser()
	{
		Console.Write("enter the id number of the Engineer: ");
		int id = readInt();

		Console.Write("enter the email of the Engineer: ");
		string email = Console.ReadLine()!;

		Console.Write("enter the amount of money per hour the Engineer gets: ");
		double cost = readDouble();

		Console.Write("enter the full name of the Engineer: ");
		string name = Console.ReadLine()!;

		Console.Write("enter the level of the Engineer (0-4): ");
		EngineerExperience level = (EngineerExperience)readInt();

		return new Engineer(id, email, cost, name, level);
	}
	/// <summary> Creates a new engineer and adds it to the database. </summary>
	static void createNewEngineer()
	{
		s_dalEngineer!.Create(getEngineerFromUser());
	}
	/// <summary> Reads an engineer from the database and prints it to the console. </summary>
	static void readEngineer()
	{
		Console.Write("enter the id of the Engineer: ");
		int id = readInt();

		Engineer? eng = s_dalEngineer!.Read(id);
		if (eng == null)
			Console.WriteLine("the Engineer does not exist");
		else
			Console.WriteLine(eng);
	}
	/// <summary> Reads all engineers from the database and prints them to the console. </summary>
	static void readAllEngineers()
	{
		foreach (var eng in s_dalEngineer!.ReadAll())
			Console.WriteLine($"> {eng}");
	}
	/// <summary> Updates an engineer in the database. </summary>
	static void updateEngineer()
	{
		s_dalEngineer!.Update(getEngineerFromUser()); // we receive the id from the user in `getEngineerFromUser`
	}
	/// <summary> Deletes an engineer from the database. </summary>
	static void deleteEngineer()
	{
		Console.Write("enter the id of the Engineer: ");
		int id = readInt();
		s_dalEngineer!.Delete(id);
	}


	/*
	 * Dependency functions
	 */

	/// <summary> Gets a dependency from the user. </summary>
	/// <returns> The dependency that the user entered. </returns>
	static Dependency getDependencyFromUser()
	{
		Console.Write("enter the id of the dependent task: ");
		int dependentId = readInt();

		Console.Write("enter the id of the task that must be done first: ");
		int dependsOnId = readInt();

		return new Dependency(dependentId, dependsOnId);
	}
	/// <summary> Creates a new dependency and adds it to the database. </summary>
	static void createNewDependency()
	{
		s_dalDependency!.Create(getDependencyFromUser());
	}
	/// <summary> Reads a dependency from the database and prints it to the console. </summary>
	static void readDependency()
	{
		Console.Write("enter the id of the Dependency: ");
		int id = readInt();

		Dependency? dep = s_dalDependency!.Read(id);
		if (dep == null)
			Console.WriteLine("the Dependency does not exist");
		else
			Console.WriteLine(dep);
	}
	/// <summary> Reads all dependencies from the database and prints them to the console. </summary>
	static void readAllDependencies()
	{
		foreach (var dep in s_dalDependency!.ReadAll())
			Console.WriteLine($"> {dep}");
	}
	/// <summary> Updates a dependency in the database. </summary>
	static void updateDependency()
	{
		Console.WriteLine("enter the id of the Dependency: ");
		int id = readInt();
		Dependency dep = getDependencyFromUser();
		s_dalDependency!.Update(dep with { Id = id });
	}
	/// <summary> Deletes a dependency from the database. </summary>
	static void deleteDependency()
	{
		Console.Write("enter the id of the Dependency: ");
		int id = readInt();
		s_dalDependency!.Delete(id);
	}
}
