namespace DalTest;

using Dal;
using DalApi;
using DO;
using System.Runtime.CompilerServices;

internal class Program
{
	private static IEngineer? s_dalEngineer = new EngineerImplementation();
	private static IDependency? s_dalDependency = new DependencyImplementation();
	private static ITask? s_dalTask = new TaskImplementation();

	public enum MainChoices
	{
		Exit,
		EngineerMenu,
		TaskMenu,
		DependencyMenu
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

	static void Main(string[] args)
	{
		try
		{
			Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependency); // initialize the data

			Console.WriteLine("Welcome to the missions managing manu");
			Console.WriteLine("Our mission is to send a spaceship to space. yay!");
			Console.WriteLine("-------------------------------------------------");

			MainChoices mainChoices;
			CrudChoices crudChoices;

			do
			{
				mainChoices = readMainMenu();

				if (mainChoices == MainChoices.Exit)
					break;

				do
				{
					crudChoices = readCrudMenu();

					if (crudChoices == CrudChoices.Exit)
						break;

					switch (crudChoices)
					{
						case CrudChoices.Create:
							switch (mainChoices)
							{
								case MainChoices.EngineerMenu: break;
								case MainChoices.TaskMenu: s_dalTask!.Create(readNewTask()); break;
								case MainChoices.DependencyMenu: break;
							}
							break;
						
						case CrudChoices.Read:
							switch (mainChoices)
							{
								case MainChoices.EngineerMenu: break;
								case MainChoices.TaskMenu: printTask(); break;
								case MainChoices.DependencyMenu: break;
							}
							break;
						
						case CrudChoices.ReadAll:
							switch (mainChoices)
							{
								case MainChoices.EngineerMenu: break;
								case MainChoices.TaskMenu:
									foreach (var task in s_dalTask!.ReadAll())
										Console.WriteLine($"> {task}");
									break;
                                case MainChoices.DependencyMenu: break;
							}
							break;
						
						case CrudChoices.Update:
							break;
						
						case CrudChoices.Delete:
							break;
						
						default:
							break;
					}
				} while (crudChoices != CrudChoices.Exit);


			} while (mainChoices != MainChoices.Exit);
		}
		catch (Exception exp)
		{
			Console.WriteLine(exp);
		}
	}


	// Menu functions:
	static MainChoices readMainMenu()
	{
		Console.WriteLine("please choose one of the following options");
		Console.WriteLine("1) to open the Engineer managing manu");
		Console.WriteLine("2) to open the Task managing manu");
		Console.WriteLine("3) to open the Dependency managing manu");
		Console.WriteLine("0) to exit");

		string? text = Console.ReadLine();
		if (text == null)
			throw new Exception("invalid input please try again");

		int choise = int.Parse(text);
		if (choise < 0 || 3 < choise)
			throw new Exception("invalid input please try again");

		return (MainChoices)choise;
	}

	static CrudChoices readCrudMenu()
	{
		Console.WriteLine("please choose one of the requested actions");
		Console.WriteLine("0) to return to the main manu");
		Console.WriteLine("1) to create new item");
		Console.WriteLine("2) to read a specific item");
		Console.WriteLine("3) to read all of the items");
		Console.WriteLine("4) to update a specific item");
		Console.WriteLine("5) to delete a specific item");

		string? text = Console.ReadLine();
		if (text == null)
			throw new Exception("invalid input please try again");

		int choise = int.Parse(text);
		if (choise < 0 || 5 < choise)
			throw new Exception("invalid input please try again");

		return (CrudChoices)choise;
	}


	// Task functions:
	static Task readNewTask()
	{
		string alias;
		string description;

		Console.Write("enter the name of the task: ");
		alias = Console.ReadLine()!;

		Console.Write("enter the description of the task: ");
		description = Console.ReadLine()!;
		
		Console.Write("what is the id of the Engineer who assigned to do the Task: ");
		int engineerId = int.Parse(Console.ReadLine()!);

		return new Task(0, alias, description, DateTime.Now, null,
			false, null, null, null, null, null, null, null, engineerId);
	}

	static void printTask()
	{
		Console.Write("enter the id of the Task: ");
		int id = int.Parse(Console.ReadLine()!);

		Task? task = s_dalTask!.Read(id);
		if (task == null)
			Console.WriteLine("the Task does not exist");
		else
			Console.WriteLine(task);
	}


	// old code:

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
