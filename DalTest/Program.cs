using Dal;
using DalApi;
using DO;

namespace DalTest;

internal class Program
{
    private static IEngineer? s_dalEngineer = new EngineerImplementation();
    private static IDependency? s_dalDependency = new DependencyImplementation();
    private static ITask? s_dalTask = new TaskImplementation();

    static void Main(string[] args)
    {
        try
        {
            ChooseManu();

        }
        catch (Exception exp)
        {

            Console.WriteLine(exp);
        }
    }

    private static void ChooseManu()
    {
        Console.WriteLine("welcome to the missions managing manu");
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
                case MainChoices.EngineerManu:
                    EngineerManu();
                    break;
                case MainChoices.TaskManu:
                    TaskManu();
                    break;
                case MainChoices.DependencyManu:
                    DependencyManu();
                    break;
                default:
                    Console.WriteLine("invalid input please try  again");
                    break;
            }

        } while (true);

    }

    private static void DependencyManu()
    {
        throw new NotImplementedException();
    }

    private static void TaskManu()
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

    private static void EngineerManu()
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
                    try { s_dalEngineer!.Update(engineerUpdate); }
                    catch (Exception exp)
                    {

                        Console.WriteLine(exp);
                    }
                    break;
                case EngineerChoices.DeleteEnginer:
                    Console.WriteLine("enter the id of the engineer you want to delete");
                    int idDelete = int.Parse(Console.ReadLine()!);
                    try { s_dalEngineer!.Delete(idDelete); }
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
}

