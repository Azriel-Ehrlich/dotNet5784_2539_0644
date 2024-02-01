using BlApi;
using BO;

namespace BlTest;

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

		
	}

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

	/*
	 * Helper functions: same to DalTest/Program.cs. maybe we should have put them in a different file
	 */

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
}
