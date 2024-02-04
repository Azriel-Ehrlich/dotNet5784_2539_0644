namespace BlTest;

using BlApi;
using BO;

internal class Program
{
	static IBl bl = new BlImplementation.Bl();
	static ProjectStatus status = ProjectStatus.plan;

	/*
     How this program works:
        1) insert all tasks
        2) insert all dependencies to the database
        3) insert dates for the tasks
        4) engineers to the database
        5) decide which engineer is assigned to which task
     */


	static void Main(string[] args)
	{
		// ascii art for BO. we are so cool :D
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine(@"  _____ _          ___  ___    _____       _   
 |_   _| |_  ___  | _ )/ _ \  |_   _|__ __| |_ 
   | | | ' \/ -_) | _ \ (_) |   | |/ -_|_-<  _|
   |_| |_||_\___| |___/\___/    |_|\___/__/\__|
");

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine("Welcome to the missions managing manu");
		Console.WriteLine("Our mission is to send a spaceship to space");
		Console.WriteLine("--------------------------------------------");

		init();
		MainChoices choice;
		do
		{
			Console.ForegroundColor = ConsoleColor.Yellow;

			Console.WriteLine("press 0 to exit the system");
			Console.WriteLine("press 1 to enter the task manu");
			Console.WriteLine("press 2 to enter the engineer manu");
			Console.WriteLine("press 3 to schedule the project");
			choice = (MainChoices)readInt();

			try
			{
				switch (choice)
				{
					case MainChoices.Exit:
						break;
					case MainChoices.Task:
						taskManu();
						break;
					case MainChoices.Engineer:
						engineerManu();
						break;
					case MainChoices.Schedule:
						if (status != ProjectStatus.plan)
							throw new BlCannotUpdateException("The project is already scheduled");
						status = ProjectStatus.schedule;
						readDates();
						break;
					default:
						throw new BlInvalidInputException("Invalid input, please try again.");
				}
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.Message);
				Console.ForegroundColor = ConsoleColor.Yellow;
			}
		} while (choice != MainChoices.Exit);

		// save the start date of the project in the database
		bl.SaveScheduledDate();

		Console.ForegroundColor = ConsoleColor.White; // reset the color
	}

	private static void engineerManu()
	{
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine("Welcome to engineer manu");
		SubMenuChoices choice;
		do
		{
			Console.WriteLine("press 0 to exit");
			Console.WriteLine("press 1 to create a new engineer");
			Console.WriteLine("press 2 to read a engineer");
			Console.WriteLine("press 3 to read all engineer");
			Console.WriteLine("press 4 to update a engineer");
			Console.WriteLine("press 5 to delete a engineer");
			choice = (SubMenuChoices)readInt();
			switch (choice)
			{
				case SubMenuChoices.Exit:
					break;

				case SubMenuChoices.Create:
					createEngineers();
					break;

				case SubMenuChoices.Read:
					Console.WriteLine("Enter the id of the engineer you want to read");
					int id = readInt();
					Engineer? task = bl.Engineer.Read(id);
					if (task is null)
						Console.WriteLine("The engineer does not exist");
					else
						Console.WriteLine(task);
					break;

				case SubMenuChoices.ReadAll:
					foreach (var e in bl.Engineer.ReadAll())
						Console.WriteLine(e);
					break;

				case SubMenuChoices.Update:
					EngineerUpdate();
					break;

				case SubMenuChoices.Delete:
					if (status == ProjectStatus.plan)
					{
						Console.WriteLine("Enter the id of the engineer you want to delete");
						id = readInt();
						BO.Engineer? e = bl.Engineer.Read(id);
						if (e is null)
						{
							Console.WriteLine("The engineer does not exist");
							break;
						}
						if (e.Task is not null)
						{
							BO.Task? t = bl.Task.Read(e.Task.Id);
							if (t is not null)
							{
								if (t.Status == BO.Status.OnTrack || t.Status == BO.Status.Done)
									throw new BlCannotDeleteException("The engineer cann't be deleted");

								t.Engineer = null;
								bl.Task.Update(t);
							}
						}
						bl.Engineer.Delete(id);
					}
					else Console.WriteLine("You can't delete tasks because the schedule is already set");
					break;

				default:
					Console.WriteLine("Invalid input, please try again.");
					break;
			}
		} while (choice != SubMenuChoices.Exit);
	}

	private static void EngineerUpdate()
	{
		Console.WriteLine("which engineer do you want to update?");
		foreach (var e in bl.Engineer.ReadAll())
			Console.WriteLine(e);

		Console.Write("enter the id number of the Engineer: ");
		int id = readInt();

		BO.Engineer oldEng = bl.Engineer.Read(id) ?? throw new BlDoesNotExistException("the Engineer does not exist");

		string? inputText;
		EngineerUpdate choice;
        Console.WriteLine("to update the email press 1");
        Console.WriteLine("to update the cost press 2");
        Console.WriteLine("to update the name press 3");
		Console.WriteLine("to update the level press 4");
        Console.WriteLine("to assign a task press 5");
		choice = (EngineerUpdate)readInt();
		switch (choice)
		{
			case BO.EngineerUpdate.Email:
				Console.Write("enter the email of the Engineer: ");
				string? email = Console.ReadLine();
				if (email == null || email == "")
					email = oldEng.Email;
				break;
			case BO.EngineerUpdate.cost:
				Console.Write("enter the amount of money per hour the Engineer gets: ");
				double cost = oldEng.Cost;
				inputText = Console.ReadLine();
				if (inputText != null && inputText != "")
				{
					if (!double.TryParse(inputText, out cost))
						throw new BlInvalidInputException();
					oldEng.Cost = cost;
				}
				break;
			case BO.EngineerUpdate.Name:

				Console.Write("enter the full name of the Engineer: ");
				string? name = Console.ReadLine();
				if (name == null || name == "")
					name = oldEng.Name;
				else
                    oldEng.Name = name;
				break;
			case BO.EngineerUpdate.ExperienceLevel:
				Console.Write("enter the level of the Engineer (0-4): ");
				EngineerExperience level = oldEng.Level;
				inputText = Console.ReadLine();
				if (inputText != null && inputText != "")
				{
					int tmp = toInt(inputText);
					if (tmp < 0 || tmp > 4)
						throw new BlInvalidInputException();
					level = (EngineerExperience)tmp;
					oldEng.Level = level;
				}
				break;
			case BO.EngineerUpdate.Task:
				Console.Write("Do you want to assign a task to work on? Y/N ");
				inputText = Console.ReadLine();
				if (inputText == "Y" || inputText == "y")
				{
					Console.WriteLine("This is the list of tasks you can assign to:");
					List<int> validIds = new();
					Task task;
					foreach (var t in bl.Task.ReadAll(t => t.Engineer is null))
					{
						task = bl.Task.Read(t.Id);
						if (task.Complexity <= oldEng.Level)
						{
							Console.WriteLine($"> {t.Id}: {t.Alias}");
							validIds.Add(t.Id);
						}
					}

					Console.Write("Enter the id of the task you want to assign to yourself: ");

					int taskId = readInt();
					while (!validIds.Contains(taskId))
					{
						Console.WriteLine("Calm down, bro, you can't do it! please choose another task");
						taskId = readInt();
					}

					task = bl.Task.Read(taskId);
					task.Engineer = new EngineerInTask() { Id = id, Name = oldEng.Name };
					bl.Task.Update(task);

					oldEng.Task = new TaskInEngineer() { Id = task.Id, Alias = task.Alias };
				}
				break;

			default:
				Console.WriteLine("nothing to update...");
				break;
		}

		bl.Engineer.Update(oldEng);
	}

	private static void taskManu()
	{
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine("Welcome to task manu");
		SubMenuChoices choice;
		do
		{
			Console.WriteLine("press 0 to exit");
			Console.WriteLine("press 1 to create a new task");
			Console.WriteLine("press 2 to read a task");
			Console.WriteLine("press 3 to read all tasks");
			Console.WriteLine("press 4 to update a task");
			Console.WriteLine("press 5 to delete a task");
			choice = (SubMenuChoices)readInt();
			switch (choice)
			{
				case SubMenuChoices.Exit:
					break;
				case SubMenuChoices.Create:
					if (status == ProjectStatus.plan)
						createTasks();
					else Console.WriteLine("You can't create new tasks because the schedule is already set");
					break;
				case SubMenuChoices.Read:
					Console.WriteLine("Enter the id of the task you want to read");
					int id = readInt();
					Task? task = bl.Task.Read(id);
					if (task is null)
						Console.WriteLine("The task does not exist");
					else
						Console.WriteLine(task);
					break;
				case SubMenuChoices.ReadAll:
					foreach (var t in bl.Task.ReadAll())
						Console.WriteLine(t);
					break;
				case SubMenuChoices.Update:
					TaskUpdate();
					break;
				case SubMenuChoices.Delete:
					if (status == ProjectStatus.plan)
					{
						Console.WriteLine("Enter the id of the task you want to delete");
						id = readInt();
						BO.Task? t = bl.Task.Read(id);
						if (t is null)
						{
							Console.WriteLine("The task does not exist");
							break;
						}
						if (t.Engineer is not null)
						{
							BO.Engineer? e = bl.Engineer.Read(t.Engineer.Id);
							if (e is not null)
							{
								e.Task = null;
								bl.Engineer.Update(e);
							}
						}
						bl.Task.Delete(id);
					}
					else Console.WriteLine("You can't delete tasks because the schedule is already set");
					break;
				default:
					Console.WriteLine("Invalid input, please try again.");
					break;
			}
		} while (choice != SubMenuChoices.Exit);


	}

	private static void TaskUpdate()
	{
		Console.WriteLine("What is the id of the task you want to update?");
		int id = readInt();
		Task? task = bl.Task.Read(id);
		if (task is null)
		{
			Console.WriteLine("The task does not exist");
			return;
		}
		Console.WriteLine("What do you want to update?");
		Console.WriteLine("1- Alias");
		Console.WriteLine("2- Description");
		Console.WriteLine("3- Engineer");
		Console.WriteLine("4- Remarks");
		Console.WriteLine("5- Deliverable");
		if (status == ProjectStatus.plan)
		{
			Console.WriteLine("6- Dependencies");
			Console.WriteLine("7- RequiredEffortTime");
			Console.WriteLine("8- Complexity");
			Console.WriteLine("9- StartDate");
			Console.WriteLine("10- CompleteTask");
		}
		TaskUpdate choice = (TaskUpdate)readInt();
		Engineer? eng = null;
		switch (choice)
		{
			case BO.TaskUpdate.Alias:
				Console.WriteLine("Enter the new alias");
				task.Alias = Console.ReadLine()!;
				bl.Task.Update(task);
				break;
			case BO.TaskUpdate.Description:
				Console.WriteLine("Enter the new description");
				task.Description = Console.ReadLine()!;
				bl.Task.Update(task);
				break;
			case BO.TaskUpdate.Engineer:
				Console.WriteLine("Enter the id of the new engineer");
				int engId = readInt();
				eng = bl.Engineer.Read(engId);
				eng!.Task = new TaskInEngineer() { Id = task.Id, Alias = task.Alias };
				bl.Engineer.Update(eng);
				task.Engineer = new EngineerInTask() { Id = eng.Id, Name = eng.Name };
				bl.Task.Update(task);
				break;
			case BO.TaskUpdate.Remraks:
				Console.WriteLine("Enter the new remarks");
				task.Remarks = Console.ReadLine()!;
				bl.Task.Update(task);
				break;
			case BO.TaskUpdate.Deliverable:
				Console.WriteLine("Enter the new deliverable");
				task.Deliverables = Console.ReadLine()!;
				 bl.Task.Update(task);
				break;
			case BO.TaskUpdate.Dependencies:
				if (status == ProjectStatus.plan)
				{
					Console.WriteLine("Enter the new dependencies");
					List<TaskInList> deps = new List<TaskInList>();
					IEnumerable<Task> previousTasks = bl.Task.ReadAll().Select(t => bl.Task.Read(t.Id)!);
					if (previousTasks.Count() > 0) // read dependencies only if there is at least 1 task
					{

						// print previous tasks
						foreach (var t in previousTasks)
							Console.WriteLine($"> {t.Id}: {t.Alias}");

						// read dependencies
						bool readMore = true;
						while (readMore is true)
						{
							Console.Write("enter the id of the task this task is dependent on: ");
							int idd = readInt();
							Task? t = bl.Task!.Read(idd);
							if (t == null)
								Console.WriteLine("the task does not exist");
							else
								deps.Add(new TaskInList() { Id = t.Id, Alias = t.Alias, Description = t.Description });

							Console.Write("Do you want to enter another dependency? (Y/N) ");
							string? ans = Console.ReadLine();
							if (ans != "y" && ans != "Y")
								readMore = false;
						}
					}
					task.Dependencies = deps;
					bl.Task.Update(task);
				}
				break;
			case BO.TaskUpdate.RequiredTimeEffort:
				Console.WriteLine("Enter the new required effort time");
				TimeSpan requiredEffortTime;
				if (!TimeSpan.TryParse(Console.ReadLine(), out requiredEffortTime))
					throw new BlInvalidInputException();
				task.RequiredEffortTime = requiredEffortTime;
                bl.Task.Update(task);

                break;
			case BO.TaskUpdate.Comlexity:
				Console.WriteLine("Enter the new complexity");
				task.Complexity = (EngineerExperience)readInt();
                bl.Task.Update(task);

                break;

			case BO.TaskUpdate.StartDate:
				Console.WriteLine("Enter the new start date");
				task.StartDate = readDateTime();
                bl.Task.Update(task);

                break;

			case BO.TaskUpdate.CompleteTask:
				Console.WriteLine("The task is done. Celebrate with donuts or coffee or whatever you like");
				task.CompleteDate = DateTime.Now;
				task.Status = Status.Done;
				// we know that the Update method will not throw an exception because
				// we already checked the dependencies so we can update the engineer:
				if (task.Engineer is not null)
				{
					eng = bl.Engineer.Read(task.Engineer.Id);
					eng!.Task = null;
					bl.Engineer.Update(eng);
				}
				bl.Task.Update(task);				 
				break;

			default:
				Console.WriteLine("nothing to update...");
				break;
		}
		bl.Task.Update(task);
	}

	/// <summary> reads all task from  the user and inserts them to the database </summary>
	static void createTasks()
	{
		bool readMore = true;
		while (readMore is not false)
		{
			createNewTask();

			Console.Write("Do you want to enter another task? (Y/N) ");
			string? ans = Console.ReadLine();
			if (ans != "y" && ans != "Y")
				readMore = false;
		}
	}

	/// <summary> reads all dates from the user and inserts them to the database </summary>
	static void readDates()
	{
		Console.WriteLine("Enter the scheduled start date of the project");
		DateTime projStart = readDateTime();
		foreach (var t in bl.Task.ReadAll(t => t.Dependencies is null))
		{
			DateTime? date = bl.SuggestedDate(t, projStart);
			if (date is null) throw new BlCannotUpdateException("The task cann't be updated");
			Console.WriteLine($"for the task {t.Id}, {t.Alias} the scheduled starting date is- {date}");
			bl.Task.UpdateScheduledDate(t.Id, (DateTime)date);
			status = ProjectStatus.execute;
		}
		foreach (var t in bl.Task.ReadAll(t => t.Dependencies is not null))
		{
			DateTime? date = bl.SuggestedDate(t, projStart);
			if (date is null) throw new BlCannotUpdateException("The task cann't be updated");
			Console.WriteLine($"for the task {t.Id}, {t.Alias} the scheduled starting date is- {date}");
			bl.Task.UpdateScheduledDate(t.Id, (DateTime)date);
			status = ProjectStatus.execute;
		}
	}



	/// <summary> reads all engineers from the user and inserts them to the database </summary>
	static void createEngineers()
	{
		bool readMore = true;
		while (readMore is not false)
		{
			createNewEngineer();

			Console.Write("Do you want to enter another engineer? (Y/N) ");
			string? ans = Console.ReadLine();
			if (ans == null || ans == "" || ans == "N" || ans == "n")
				readMore = false;
		}
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
		//return toInt(Console.ReadLine());
		int i = 0;
		bool valid = false;
		while (!valid)
		{
			try
			{
				i = toInt(Console.ReadLine());
				valid = true;
			}
			catch (BlInvalidInputException ex)
			{
				Console.WriteLine(ex.Message);
				valid = false;
			}
		}
		return i;
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
		//return toDateTime(Console.ReadLine());
		DateTime d = new DateTime();
		bool valid = false;
		while (!valid)
		{
			try
			{
				d = toDateTime(Console.ReadLine());
				valid = true;
			}
			catch (BlInvalidInputException ex)
			{
				Console.WriteLine(ex.Message);
				valid = false;
			}
		}
		return d;
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

	/// <summary> Creates a new task and adds it to the database. </summary>
	static void createNewTask()
	{
		Console.Write("enter the name of the task: ");
		string alias = Console.ReadLine()!;

		Console.Write("enter the description of the task: ");
		string description = Console.ReadLine()!;

		Console.Write("enter the complexity of the task (0-4): ");
		EngineerExperience complexity = (EngineerExperience)readInt();

		Console.Write("enter the required effort time of the task (days): ");
		TimeSpan requiredEffortTime;
		if (!TimeSpan.TryParse(Console.ReadLine(), out requiredEffortTime))
			throw new BlInvalidInputException();

		List<TaskInList> deps = new List<TaskInList>();
		IEnumerable<Task> previousTasks = bl.Task!.ReadAll().Select(t => bl.Task.Read(t.Id)!);
		if (previousTasks.Count() > 0) // read dependencies only if there is at least 1 task
		{
			Console.Write("Is this task dependent on other tasks? (Y/N) ");
			string? ans = Console.ReadLine();
			if (ans == "Y" || ans == "y")
			{
				// print previous tasks
				foreach (var task in previousTasks)
					Console.WriteLine($"> {task.Id}: {task.Alias}");

				// read dependencies
				bool readMore = true;
				while (readMore)
				{
					Console.Write("enter the id of the task this task is dependent on: ");
					int id = readInt();
					Task? task = bl.Task!.Read(id);
					if (task == null)
						Console.WriteLine("the task does not exist");
					else
						deps.Add(new TaskInList() { Id = task.Id, Alias = task.Alias, Description = task.Description });

					Console.Write("Do you want to enter another dependency? (Y/N) ");
					ans = Console.ReadLine();
					if (ans != "y" && ans != "Y")
						readMore = false;
				}

			}
		}

		bl.Task!.Create(new Task()
		{
			Alias = alias,
			Description = description,
			RequiredEffortTime = requiredEffortTime,
			Complexity = complexity,
			Dependencies = deps
		});
	}
}
