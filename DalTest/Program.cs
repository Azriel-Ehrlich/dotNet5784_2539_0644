﻿namespace DalTest;

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
			throw new Exception("invalid input");
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
		Console.WriteLine("4) update a specific item (if you don't want to change a field, leave it empty).");
		Console.WriteLine("5) delete a specific item");

		CrudChoices choose = (CrudChoices)readInt();
		if (choose < CrudChoices.Exit || choose > CrudChoices.Delete)
			throw new Exception("invalid input");
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

		string? inputText;

		inputText = Console.ReadLine();
		DateTime? startDate = null, deadLine = null;
		if (inputText != null && inputText != "")
			startDate = DateTime.Parse(inputText);

		Console.WriteLine("enter deadLine");
		inputText = Console.ReadLine();
		if (inputText != null && inputText != "")
			deadLine = DateTime.Parse(inputText);

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

		Task oldTask = s_dalTask!.Read(id) ?? throw new Exception("the Task does not exist");

		Console.Write("enter the name of the task: ");
		string? alias = Console.ReadLine();
		if (alias == null || alias == "")
			alias = oldTask.Alias;

		Console.Write("enter the description of the task: ");
		string? description = Console.ReadLine();
		if (description == null || description == "")
			description = oldTask.Description;

		Console.Write("what is the id of the Engineer who assigned to do the Task: ");
		int? engineerId = oldTask.EngineerId;
		string? inputText = Console.ReadLine();
		if (inputText != null && inputText != "")
		{
			int tmp;
			if (!int.TryParse(inputText, out tmp))
				throw new Exception("invalid input");
			engineerId = tmp;
		}

		DateTime? startDate = oldTask.StartDate;
		Console.Write("enter start date: ");
		inputText = Console.ReadLine();
		if (inputText != null && inputText != "")
			startDate = DateTime.Parse(inputText);

		DateTime? deadLine = oldTask.DeadLineDate;
		Console.Write("enter deadLine: ");
		inputText = Console.ReadLine();
		if (inputText != null && inputText != "")
			deadLine = DateTime.Parse(inputText);

		TimeSpan? requiredEffortTime = deadLine - startDate;

		Console.Write("is this task is a milestone? ");
		bool isMilestone = oldTask.IsMilestone;
		inputText = Console.ReadLine();
		if (inputText != null && inputText != "")
		{
			bool tmp;
			if (!bool.TryParse(inputText, out tmp))
				throw new Exception("invalid input");
			isMilestone = tmp;
		}

		Console.Write("enter the complexity of the task (0-4): ");
		EngineerExperience? complexity = oldTask.Complexity;
		inputText = Console.ReadLine();
		if (inputText != null && inputText != "")
		{
			int tmp;
			if (!int.TryParse(inputText, out tmp))
				throw new Exception("invalid input");
			if (tmp < 0 || tmp > 4)
				throw new Exception("invalid input");
			complexity = (EngineerExperience)tmp;
		}

		s_dalTask!.Update(new Task(id, alias, description, DateTime.Now,
			requiredEffortTime, isMilestone, complexity, startDate,
			null, deadLine, null, "", "", engineerId));
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


	/// <summary> Creates a new engineer and adds it to the database according to user. </summary>
	static void createNewEngineer()
	{
		Console.Write("enter the id number of the Engineer: ");
		int id = readInt();

		Console.Write("enter the email of the Engineer: ");
		string email = Console.ReadLine()!;

		Console.Write("enter the amount of money per hour the Engineer gets: ");
		double cost;
		if (!double.TryParse(Console.ReadLine(), out cost))
			throw new Exception("invalid input");

		Console.Write("enter the full name of the Engineer: ");
		string name = Console.ReadLine()!;

		Console.Write("enter the level of the Engineer (0-4): ");
		EngineerExperience level = (EngineerExperience)readInt();

		s_dalEngineer!.Create(new Engineer(id, email, cost, name, level));
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
	/// <summary> Updates an engineer in the database according to user input. </summary>
	static void updateEngineer()
	{
		Console.Write("enter the id number of the Engineer: ");
		int id = readInt();

		Engineer oldEng = s_dalEngineer!.Read(id) ?? throw new Exception("the Engineer does not exist");

		string? inputText;

		Console.Write("enter the email of the Engineer: ");
		string? email = Console.ReadLine();
		if (email == null || email == "")
			email = oldEng.Email;

		Console.Write("enter the amount of money per hour the Engineer gets: ");
		double cost = oldEng.Cost;
		inputText = Console.ReadLine();
		if (inputText != null && inputText != "")
		{
			if (!double.TryParse(inputText, out cost))
				throw new Exception("invalid input");
		}

		Console.Write("enter the full name of the Engineer: ");
		string? name = Console.ReadLine();
		if (name == null || name == "")
			name = oldEng.Name;	

		Console.Write("enter the level of the Engineer (0-4): ");
		EngineerExperience level = oldEng.Level;
		inputText = Console.ReadLine();
		if (inputText != null && inputText != "")
		{
			int tmp;
			if (!int.TryParse(inputText, out tmp))
				throw new Exception("invalid input");
			if (tmp < 0 || tmp > 4)
				throw new Exception("invalid input");
			level = (EngineerExperience)tmp;
		}

		s_dalEngineer!.Update(new Engineer(id, email, cost, name, level));
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

	/// <summary> Creates a new dependency and adds it to the database according to user input. </summary>
	static void createNewDependency()
	{
		Console.Write("enter the id of the dependent task: ");
		int dependentId = readInt();

		Console.Write("enter the id of the task that must be done first: ");
		int dependsOnId = readInt();

		s_dalDependency!.Create(new Dependency(dependentId, dependsOnId));
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
	/// <summary> Updates a dependency in the database according to user input. </summary>
	static void updateDependency()
	{
		Console.WriteLine("enter the id of the Dependency: ");
		int id = readInt();

		Dependency oldDep = s_dalDependency!.Read(id) ?? throw new Exception("the Dependency does not exist");

		int? dependentId = oldDep.DependentTask;
		int? dependsOnId = oldDep.DependsOnTask;
		string? inputText;

		Console.Write("enter the id of the dependent task: ");
		inputText = Console.ReadLine();
		if (inputText != null && inputText != "")
			dependentId = int.Parse(inputText);

		Console.Write("enter the id of the task that must be done first: ");
		inputText = Console.ReadLine();
		if (inputText != null && inputText != "")
			dependsOnId = int.Parse(inputText);

		s_dalDependency!.Update(new Dependency(id, dependentId, dependsOnId));
	}
	/// <summary> Deletes a dependency from the database. </summary>
	static void deleteDependency()
	{
		Console.Write("enter the id of the Dependency: ");
		int id = readInt();
		s_dalDependency!.Delete(id);
	}
}
