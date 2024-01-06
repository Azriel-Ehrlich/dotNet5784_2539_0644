namespace DalTest;

using Dal;
using DalApi;
using DO;

internal class Program
{
	private static IEngineer? s_dalEngineer = new EngineerImplementation();
	private static IDependency? s_dalDependency = new DependencyImplementation();
	private static ITask? s_dalTask = new TaskImplementation();

	public enum MainChoices
	{
		Exit,
		Engineer,
		Task,
		Dependency
	}
	public enum CrudChoices
	{
		Exit,
		Create,
		Read,
		ReadAll,
		Update,
		Delete
	}

	/// <summary> The main function of the program. </summary>
	/// <param name="args"> The arguments of the program. </param>
	static void Main(string[] args)
	{
		Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependency); // initialize the data

		Console.WriteLine("Welcome to the missions managing manu");
		Console.WriteLine("Our mission is to send a spaceship to space. yay!");
		Console.WriteLine("-------------------------------------------------");

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

						funcs[mainChoice][crudChoice](); // call the function with cool syntax :)
					}
					catch (Exception exp)
					{
						Console.WriteLine("Error: " + exp.Message);
					}
				}
			}
			catch (Exception exp)
			{
				Console.WriteLine("Error: " + exp.Message);
			}
		}
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
		Console.WriteLine("Main manu:");
		Console.WriteLine("Please choose one of the following options:");
		Console.WriteLine("1) open the Engineer managing manu");
		Console.WriteLine("2) open the Task managing manu");
		Console.WriteLine("3) open the Dependency managing manu");
		Console.WriteLine("0) exit");

		MainChoices choose =(MainChoices)readInt();
		if (choose < MainChoices.Exit || choose > MainChoices.Dependency)
            throw new Exception("invalid input please try again");
		return choose;
	}

	/// <summary> Gets the choice of the user for the CRUD menu. </summary>
	/// <param name="mainChoice"> The choice of the user for the main menu. used to print the correct menu. </param>
	/// <returns> The choice of the user for the CRUD menu. </returns>
	static CrudChoices getCrudMenuChoice(MainChoices mainChoice)
	{
		// print message about the current menu:
		Console.WriteLine($"{mainChoice} managing manu:");
		Console.WriteLine("Please choose one of the requested actions:");
		Console.WriteLine("0) return to the main manu");
		Console.WriteLine("1) create new item");
		Console.WriteLine("2) read a specific item");
		Console.WriteLine("3) read all of the items");
		Console.WriteLine("4) update a specific item");
		Console.WriteLine("5) delete a specific item");

		CrudChoices choose= (CrudChoices)readInt();
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

		return new Task(alias, description, engineerId);
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


	// code of AE:
	/*
	private static void ChooseMenu()
	{
		Console.WriteLine("Welcome to the missions managing manu");
		Console.WriteLine("Our mission is to send a spaceship to space. yay!");
		Console.WriteLine("-------------------------------------------------");

		do
		{
			Console.WriteLine("please choose one of the following options");
			Console.WriteLine("press 1 to open the Engineer managing manu");
			Console.WriteLine("press 2 to open the Task managing manu");
			Console.WriteLine("press 3 to open the Dependency managing manu");
			Console.WriteLine("press 0 to exit");
			MainChoices choise = (MainChoices)int.Parse(Console.ReadLine()!);

			switch (choise)
			{
				case MainChoices.Exit:
					return;

				case MainChoices.EngineerMenu:
					EngineerMenu();
					break;

				case MainChoices.TaskMenu:
					TaskMenu();
					break;

				case MainChoices.DependencyMenu:
					DependencyMenu();
					break;

				default:
					Console.WriteLine("invalid input please try again");
					break;
			}

		} while (true);
	}

	private static void DependencyMenu()
	{
		throw new NotImplementedException();
	}

	private static void TaskMenu()
	{
		TaskChoices choise;
		Console.WriteLine("welcome to the Task managing manu");
		do
		{
			Console.WriteLine("please choose one of the requested actions");
			Console.WriteLine("press 0 to return to the main manu");
			Console.WriteLine("press 1 to add a new Task");
			Console.WriteLine("press 2 to see a specific Taks");
			Console.WriteLine("press 3 to see all of the Tasks");
			Console.WriteLine("press 4 to update a specific Task");
			Console.WriteLine("press 5 to delete a specific Task");
			choise = (TaskChoices)int.Parse(Console.ReadLine()!);
			switch (choise)
			{
				case TaskChoices.Exit:
					break;
				case TaskChoices.AddTask:
					Console.WriteLine("enter the name of the task");
					string alias = Console.ReadLine()!;
					Console.WriteLine("enter the description of the task");
					string description = Console.ReadLine()!;
					Console.WriteLine("is this task is a milestone?");
					bool isMilestone = bool.Parse(Console.ReadLine()!);
					Console.WriteLine("enter the complexity of the task");
					EngineerExperience complexity = (EngineerExperience)int.Parse(Console.ReadLine()!);
					Console.WriteLine("enter the date to start working on the project");
					DateTime shedualdtDate = DateTime.Parse(Console.ReadLine()!);
					Console.WriteLine("enter the date to finish the project");
					DateTime deadLineDate = DateTime.Parse(Console.ReadLine()!);
					TimeSpan requiredEffortTime = deadLineDate - shedualdtDate;
					Console.WriteLine("what is the id of the Engineer who assigned to do the Task");
					int engineerId = int.Parse(Console.ReadLine()!);


					break;
				case TaskChoices.GetTask:
					break;
				case TaskChoices.GetAllTasks:
					break;
				case TaskChoices.UpdateTask:
					break;
				case TaskChoices.DeleteTask:
					break;
				default:
					Console.WriteLine("invalid input please try again");
					break;
			}
		} while (choise != TaskChoices.Exit);
	}

	private static void EngineerMenu()
	{
		EngineerChoices choise;
		Console.WriteLine("welcome to the Engineer managing manu");

		do
		{
			Console.WriteLine("please choose one of the requested action");
			Console.WriteLine("press 0 to return to the main manu");
			Console.WriteLine("press 1 to add a new Engineer");
			Console.WriteLine("press 2 to get info about a specific Engineer");
			Console.WriteLine("press 3 to get the info about all of the Engineers");
			Console.WriteLine("press 4 to update a specific Engineer");
			Console.WriteLine("press 5 to delete a specific Engineer");
			choise = (EngineerChoices)int.Parse(Console.ReadLine()!);
			switch (choise)
			{
				case EngineerChoices.Exit:
					break;

				case EngineerChoices.AddEngineer:
					try
					{
						Console.WriteLine("enter the id number of the Engineer");
						int idAdd = int.Parse(Console.ReadLine()!);
						Console.WriteLine("enter the email of the Engineer");
						string email = Console.ReadLine()!;
						Console.WriteLine("enter the amount of money per hour the Engineer gets");
						double cost = double.Parse(Console.ReadLine()!);
						Console.WriteLine("enter the full name of the Engineer");
						string name = Console.ReadLine()!;
						Console.WriteLine("enter the level of the Engineer");
						EngineerExperience level = (EngineerExperience)int.Parse(Console.ReadLine()!);
						Engineer newEngineer = new(idAdd, email, cost, name, level);
						s_dalEngineer!.Create(newEngineer);
					}
					catch (Exception exp)
					{

						Console.WriteLine(exp);
					}
					break;

				case EngineerChoices.GetEngineer:
					Console.WriteLine("enter the id of the Engineer");
					int id = int.Parse(Console.ReadLine()!);
					Engineer? readEngineer = s_dalEngineer!.Read(id);
					if (readEngineer == null)
						Console.WriteLine("the Engineer does not exist");
					else
						Console.WriteLine(readEngineer);
					break;

				case EngineerChoices.GetAllEngineers:
					Console.WriteLine(s_dalEngineer!.ReadAll());
					break;

				case EngineerChoices.UpdateEngineer:
					Console.WriteLine("enter the id of the Engineer");
					int idUpdate = int.Parse(Console.ReadLine()!);
					Console.WriteLine("now put in the updated info about him");
					Console.WriteLine("enter the email of the Engineer");
					string emailUpdate = Console.ReadLine()!;
					Console.WriteLine("enter the amount of money per hour the Engineer gets");
					double costUpdate = double.Parse(Console.ReadLine()!);
					Console.WriteLine("enter the full name of the Engineer");
					string nameUpdate = Console.ReadLine()!;
					Console.WriteLine("enter the level of the Engineer");
					EngineerExperience levelUpdate = (EngineerExperience)int.Parse(Console.ReadLine()!);
					Engineer engineerUpdate = new(idUpdate, emailUpdate, costUpdate, nameUpdate, levelUpdate);
					try 
					{
						s_dalEngineer!.Update(engineerUpdate); 
					}
					catch (Exception exp)
					{
						Console.WriteLine(exp);
					}
					break;

				case EngineerChoices.DeleteEnginer:
					Console.WriteLine("enter the id of the engineer you want to delete");
					int idDelete = int.Parse(Console.ReadLine()!);
					try
					{
						s_dalEngineer!.Delete(idDelete); 
					}
					catch (Exception exp)
					{
						Console.WriteLine(exp);
					}
					break;

				default:
					Console.WriteLine("invalid input try again");
					break;
			}
		} while (choise != EngineerChoices.Exit);
	}
	*/
}
