namespace BlTest;

using BlApi;
using BO;


internal class Program
{
	static IBl bl = new BlImplementation.Bl();


	static void Main(string[] args)
	{
		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine("Welcome to the missions managing manu");
		Console.WriteLine("Our mission is to send a spaceship to space");
		Console.WriteLine("--------------------------------------------");

		init();

		// Dictionary of functions to call for each menu choice with cool syntax:
		Dictionary<MainChoices, Dictionary<SubMenuChoices, Action>> funcs = new()
		{
			{
				MainChoices.Engineer,
				new Dictionary<SubMenuChoices, Action>()
				{
					{ SubMenuChoices.Create, createNewEngineer },
					{ SubMenuChoices.Read, readEngineer },
					{ SubMenuChoices.ReadAll, readAllEngineers },
					{ SubMenuChoices.Update, updateEngineer },
					{ SubMenuChoices.Delete, deleteEngineer },
				}
			},
			{
				MainChoices.Task,
				new Dictionary<SubMenuChoices, Action>()
				{
					{ SubMenuChoices.Create, createNewTask },
					{ SubMenuChoices.Read, readTask },
					{ SubMenuChoices.ReadAll, readAllTasks },
					{ SubMenuChoices.Update, updateTask },
					{ SubMenuChoices.Delete, deleteTask },
					{ SubMenuChoices.UpdateScheduledDate, updateScheduledDate }
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

				SubMenuChoices crudChoice = SubMenuChoices.Create; // default value so the loop will start
				while (crudChoice != SubMenuChoices.Exit)
				{
					try
					{
						crudChoice = getSubMenuChoice(mainChoice);
						if (crudChoice == SubMenuChoices.Exit)
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

	// the next functions are same to DalTest/Program.cs. maybe we should have put them in a different file


	/// <summary> Gets the choice of the user for the main menu. </summary>
	/// <returns> The choice of the user for the main menu. </returns>
	static MainChoices getMainMenuChoice()
	{
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine("Main manu:");
		Console.WriteLine("Please choose one of the following options:");
		Console.WriteLine("1) open the Engineer managing manu");
		Console.WriteLine("2) open the Task managing manu");
		Console.WriteLine("0) exit");

		MainChoices choice = (MainChoices)readInt();
		if (choice < MainChoices.Exit || choice > MainChoices.Task)
			throw new BlInvalidInputException();
		return choice;
	}

	/// <summary> Gets the choice of the user for the CRUD menu. </summary>
	/// <param name="mainChoice"> The choice of the user for the main menu. used to print the correct menu. </param>
	/// <returns> The choice of the user for the CRUD menu. </returns>
	static SubMenuChoices getSubMenuChoice(MainChoices mainChoice)
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

		if (mainChoice == MainChoices.Task)
			Console.WriteLine("6) update the date of a specific task");

		SubMenuChoices choose = (SubMenuChoices)readInt();
		if (mainChoice == MainChoices.Task && choose == SubMenuChoices.UpdateScheduledDate)
			return choose;
		if (choose < SubMenuChoices.Exit || choose > SubMenuChoices.Delete)
			throw new BlInvalidInputException();
		return choose;

	}

	/// <summary> Asks the user if he wants to create initial data and creates it if he does. </summary>
	private static void init()
	{
		// same to the one in DalTest... thats because the access modifier is internal
		Console.ForegroundColor = ConsoleColor.Magenta;

		Console.Write("Would you like to create Initial data? (Y/N) ");
		string? ans;
		do ans = Console.ReadLine();
		while (ans == null);
		if (ans == "Y" || ans == "y")
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Creating initial data...");
			DalTest.Initialization.Do(); // initialize the data
		}
	}

	/// <summary> Reads an integer from the user. </summary>
	/// <returns> The integer that the user entered. </returns>
	/// <exception cref="BlInvalidInputException"> Thrown when the user entered an invalid input. </exception>
	static int readInt()
	{
		return toInt(Console.ReadLine());
	}

	/// <summary> Converts a string to an integer. </summary>
	/// <param name="str"> The str to convert. </param>
	/// <returns> The integer from the string. </returns>
	/// <exception cref="BlInvalidInputException"> Thrown when the user entered an invalid input. </exception>
	static int toInt(string? str)
	{
		int i;
		if (!int.TryParse(str, out i))
			throw new BlInvalidInputException();
		return i;
	}

	/// <summary> Reads a date from the user. </summary>
	static DateTime readDateTime()
	{
		return toDateTime(Console.ReadLine());
	}
	/// <summary> Converts a string to a date. </summary>
	/// <param name="str"> The str to convert. </param>
	/// <returns> The date from the string. </returns>
	/// <exception cref="BlInvalidInputException"> Thrown when the user entered an invalid input. </exception>
	static DateTime toDateTime(string? str)
	{
		DateTime d;
		if (!DateTime.TryParse(str, out d))
			throw new BlInvalidInputException();
		return d;
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
			throw new BlInvalidInputException();

		Console.Write("enter the full name of the Engineer: ");
		string name = Console.ReadLine()!;

		Console.Write("enter the level of the Engineer (0-4): ");
		EngineerExperience level = (EngineerExperience)readInt();

		// we do not need read the tasks of the engineer because he has no tasks yet

		bl.Engineer!.Create(new Engineer
		{
			Id = id,
			Email = email,
			Cost = cost,
			Name = name,
			Level = level
		});
	}
	/// <summary> Reads an engineer from the database and prints it to the console. </summary>
	static void readEngineer()
	{
		Console.Write("enter the id of the Engineer: ");
		int id = readInt();

		Engineer? eng = bl.Engineer!.ReadEngineer(id);
		if (eng == null)
			Console.WriteLine("the Engineer does not exist");
		else
			Console.WriteLine(eng);
	}
	/// <summary> Reads all engineers from the database and prints them to the console. </summary>
	static void readAllEngineers()
	{
		foreach (var eng in bl.Engineer!.ReadAll())
			Console.WriteLine($"> {eng}");
	}
	/// <summary> Updates an engineer in the database according to user input. </summary>
	static void updateEngineer()
	{
		Console.Write("enter the id number of the Engineer: ");
		int id = readInt();

		Engineer oldEng = bl.Engineer!.ReadEngineer(id) ?? throw new BlDoesNotExistException("the Engineer does not exist");

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
				throw new BlInvalidInputException();
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
			int tmp = toInt(inputText);
			if (tmp < 0 || tmp > 4)
				throw new BlInvalidInputException();
			level = (EngineerExperience)tmp;
		}

		// TODO: read the tasks of the engineer and update them

		bl.Engineer!.Update(new Engineer { Id = id, Email = email, Cost = cost, Name = name, Level = level });
	}
	/// <summary> Deletes an engineer from the database. </summary>
	static void deleteEngineer()
	{
		Console.Write("enter the id of the Engineer: ");
		int id = readInt();
		bl.Engineer!.Delete(id);
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
		Engineer? eng = bl.Engineer!.ReadEngineer(engineerId) ?? throw new BlDoesNotExistException("the Engineer does not exist");

		Console.WriteLine("enter start date");
		DateTime startDate = readDateTime();

		Console.WriteLine("enter deadline");
		DateTime deadLine = readDateTime();
		
		Console.WriteLine("enter scheduled date");
		DateTime scheduledDate = readDateTime();
		
		Console.WriteLine("enter forecast date");
		DateTime forecastDate = readDateTime();
		
		Console.WriteLine("enter complete date");
		DateTime completeDate = readDateTime();
		
		Console.Write("enter the complexity of the task (0-4): ");
		EngineerExperience complexity = (EngineerExperience)readInt();
		
		TimeSpan requiredEffortTime = deadLine - startDate;


		return new Task()
		{
			Alias = alias,
			Description = description,
			RequiredEffortTime = requiredEffortTime,
			StartDate = startDate,
			ScheduledDate = scheduledDate,
			ForecastDate = forecastDate,
			CompleteDate = completeDate,
			Complexity = complexity,
			Engineer = eng
		};
	}
	/// <summary> Creates a new task and adds it to the database. </summary>
	static void createNewTask()
	{
		bl.Task!.Create(getTaskFromUser());
	}
	/// <summary> Reads a task from the database and prints it to the console. </summary>
	static void readTask()
	{
		Console.Write("enter the id of the Task: ");
		int id = readInt();

		Task? task = bl.Task!.Read(id);
		if (task == null)
			Console.WriteLine("the Task does not exist");
		else
			Console.WriteLine(task);
	}
	/// <summary> Reads all tasks from the database and prints them to the console. </summary>
	static void readAllTasks()
	{
		foreach (var task in bl.Task!.ReadAll())
			Console.WriteLine($"> {task}");
	}
	/// <summary> Updates a task in the database. </summary>
	static void updateTask()
	{
		Console.Write("enter the id of the Task: ");
		int id = readInt();

		Task oldTask = bl.Task!.Read(id) ?? throw new BlDoesNotExistException("the Task does not exist");

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
			engineerId = toInt(inputText);

		DateTime? startDate = oldTask.StartDate;
		Console.Write("enter start date: ");
		inputText = Console.ReadLine();
		if (inputText != null && inputText != "")
			startDate = DateTime.Parse(inputText);

		DateTime? deadLine = oldTask.StartDate;
		Console.Write("enter deadLine: ");
		inputText = Console.ReadLine();
		if (inputText != null && inputText != "")
			deadLine = DateTime.Parse(inputText);

		TimeSpan? requiredEffortTime = deadLine - startDate;

		/*
		Console.Write("is this task is a milestone? ");
		bool isMilestone = oldTask.IsMilestone;
		inputText = Console.ReadLine();
		if (inputText != null && inputText != "")
		{
			bool tmp;
			if (!bool.TryParse(inputText, out tmp))
				throw new DalInvalidInputException();
			isMilestone = tmp;
		}
		*/

		Console.Write("enter the complexity of the task (0-4): ");
		EngineerExperience? complexity = oldTask.Complexity;
		inputText = Console.ReadLine();
		if (inputText != null && inputText != "")
		{
			int tmp = toInt(inputText);
			if (tmp < 0 || tmp > 4)
				throw new BlInvalidInputException();
			complexity = (EngineerExperience)tmp;
		}

		bl.Task!.Update(new Task(id, alias, description, DateTime.Now,
			requiredEffortTime, isMilestone, complexity, startDate,
			null, deadLine, null, "", "", engineerId));
	}
	/// <summary> Deletes a task from the database. </summary>
	static void deleteTask()
	{
		Console.Write("enter the id of the Task: ");
		int id = readInt();
		bl.Task!.Delete(id);
	}

	/// <summary> Updates the scheduled date of a task in the database. </summary>
	static void updateScheduledDate()
	{
		Console.Write("enter the id of the Task: ");
		int id = readInt();

		Console.Write("enter the new scheduled date: ");
		string input = Console.ReadLine()!;
		DateTime date = new DateTime();
		if (input is not null && input != "")
			if (!DateTime.TryParse(input, out date))
				throw new BlInvalidInputException();

		bl.Task!.UpdateScheduledDate(id, date);
	}
}
