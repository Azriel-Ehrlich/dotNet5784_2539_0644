namespace BlTest;

using BlApi;
using BO;
using System.Numerics;

internal class Program
{
    static IBl bl = new BlImplementation.Bl();

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

        Console.WriteLine("Please enter all the tasks:");
        createTasks();

        Console.WriteLine("Insert schedueled starting dates for all the tasks:");
        readDates();

        Console.WriteLine("Enter all engineers:");
        createEngineers();

        Console.WriteLine("And now select which engineer is assigned to which task:");
        assignEngineersToTasks();

        Console.WriteLine("congratulations! you have successfully inserted all the data to the database");
        Console.WriteLine("Have a nice day and celebrate with delicious cake :)");

        Console.ForegroundColor = ConsoleColor.White; // reset the color
    }

    /// <summary> reads all task from  the user and inserts them to the database </summary>
    static void createTasks()
    {
        bool readMore = true;
        while (readMore)
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
            bl.Task.UpdateScheduledDate(t.Id, projStart);
        }
        foreach(var t in bl.Task.ReadAll(t => t.Dependencies is not null))
        {
            DateTime? date = suggestedDate(t);
            if(date is null) throw new BlCannotUpdateException("The task cann't be updated");
            Console.WriteLine($"for the task task {t.Id} we recommand you to choose the date {date}");
            Console.WriteLine("if you have any objection press n");
            string? ans = Console.ReadLine();
            if(ans=="n"||ans=="N")
            {
                Console.WriteLine("enter the date you want");
                date = readDateTime();
               bl.Task.UpdateScheduledDate(t.Id, (DateTime)date);
            }
            else
            {
                bl.Task.UpdateScheduledDate(t.Id, (DateTime)date);
            }
        }
        //TODO: save the start date of the project in the database

    }

    private static DateTime? suggestedDate(BO.Task? t)
    {
        DateTime? date = DateTime.MinValue;
        foreach (var dep in t!.Dependencies!)
        {
           BO.Task? depTask = bl.Task!.Read(dep.Id);
            if(depTask!.ScheduledDate is null) throw new BlCannotUpdateException("The task cann't be updated");
            DateTime? depSDate = depTask.ScheduledDate+depTask.RequiredEffortTime;
            DateTime? depTDate = depTask.StartDate + depTask.RequiredEffortTime;
            if (depSDate > date)
                date = depSDate;
            if (depTDate > date&&depTDate is not null)
                date = depTDate;
        }
        return (date>DateTime.MinValue)?date:null;
    }

    /// <summary> reads all engineers from the user and inserts them to the database </summary>
    static void createEngineers()
        {
            bool readMore = true;
            while (readMore)
            {
                createNewEngineer();

                Console.Write("Do you want to enter another engineer? (Y/N) ");
                string? ans = Console.ReadLine();
                if (ans == null || ans == "" || ans == "N" || ans == "n")
                    readMore = false;
            }
        }

        /// <summary> assign engineers to tasks </summary>
        static void assignEngineersToTasks()
        {
            Console.WriteLine("Your engineers:");
            foreach (var eng in bl.Engineer!.ReadAll())
                Console.WriteLine($"> {eng.Id}: {eng.Name}, {eng.Level}");

            Console.WriteLine("Your tasks:");
            foreach (var task in bl.Task!.ReadAll())
            {
                Console.WriteLine($"Current task: {task.Alias}");
                Console.Write("Enter the id of the engineer assigned to the task: ");
                int engId;
                Engineer? eng = null;
                while (eng is null)
                {
                    engId = readInt();
                    eng = bl.Engineer!.ReadEngineer(engId);
                    if (eng is null)
                        Console.WriteLine("The engineer does not exist");
                }
                task.Engineer = new EngineerInTask() { Id = eng.Id, Name = eng.Name };
                bl.Task!.Update(task);
            }
        }


        // the next functions are same to DalTest/Program.cs. maybe we should have put them in a different file

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

            Console.WriteLine("enter the required effort time of the task");
            TimeSpan requiredEffortTime;
            if (!TimeSpan.TryParse(Console.ReadLine(), out requiredEffortTime))
                throw new BlInvalidInputException();

            List<TaskInList> deps = new List<TaskInList>();
            IEnumerable<Task> previousTasks = bl.Task!.ReadAll();
            if (previousTasks.Count() > 1) // read dependencies only if there is at least 1 task
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
