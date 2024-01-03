namespace DalTest;

using DalApi;
using DO;

public static class Initialization
{
    private static ITask? s_dalTask;
    private static IEngineer? s_dalEngineer;
    private static IDependency? s_dalDependency;
    private static readonly Random s_rand = new();

    private static readonly string[] NAMES =
    {
        "Abraham", "Stewart", "Warren", "Anthony", "Alan",
        "Sean", "Jonathan", "Nathan", "Matt", "Stephen",
        "Christian", "David", "Brandon", "Max",  "Adrian",
        "Piers", "Lucas", "Robert", "Joseph", "Blake"
    }; // Thanks to AI for the list of names
    private static readonly int MIN_ID = 20000000, MAX_ID = 40000000;

    private static string getRandomText()
    {
        int length = s_rand.Next(10, 20);
        string text = "";
        for (int i = 0; i < length; i++)
            text += (char)s_rand.Next('a', 'z');
        return text;      
    }
    private static DateTime getRandomData()
    {
        int year = s_rand.Next(2000, 2024);
        int month = s_rand.Next(1, 13);
        int day = s_rand.Next(1, 29);
        return new DateTime(year, month, day);
    }
    private static TimeSpan getRandomTimeSpan()
    {
        int hours = s_rand.Next(0, 24);
        int minutes = s_rand.Next(0, 60);
        int seconds = s_rand.Next(0, 60);
        return new TimeSpan(hours, minutes, seconds);
    }
    private static EngineerExperience getRandomEngineerExperience()
    {
        return (EngineerExperience)s_rand.Next(Enum.GetNames(typeof(EngineerExperience)).Length);
    }

    private static void createEngineers()
    {
        int _id;
        foreach (var _name in NAMES)
        {
            do _id = s_rand.Next(MIN_ID, MAX_ID);
            while (s_dalEngineer!.Read(_id) != null);

            s_dalEngineer.Create(new Engineer(
                _id, _name + "@eng.com", s_rand.Next(60, 200), _name,
                getRandomEngineerExperience()
            ));
        }
    }

    private static void createTasks()
    {
        foreach (var eng in s_dalEngineer!.ReadAll())
        {
            string alias = getRandomText();
            string description = getRandomText();
            DateTime createdAtDate = getRandomData();
            TimeSpan requiredEffortTime = getRandomTimeSpan();
            bool isMilestone = s_rand.Next(2) == 0;
            EngineerExperience complexity = getRandomEngineerExperience();
            DateTime startDate = getRandomData();
            DateTime schedualdDate = getRandomData();
            DateTime deadLineDate = getRandomData();
            DateTime completeDate = getRandomData();
            string deliverables = getRandomText();
            string remarks = getRandomText();
            int engineerId = eng.Id;

            s_dalTask!.Create(new Task(
                0, alias, description, createdAtDate,
                requiredEffortTime, isMilestone, complexity,
                startDate, schedualdDate, deadLineDate, completeDate,
                deliverables, remarks, engineerId)
            );
        }
    }

    private static void createDependency()
    {
        foreach (var task in s_dalTask!.ReadAll())
        {
            int taskId = task.Id;
            int dependentTaskId = s_rand.Next(s_dalTask.ReadAll().Count);

            Dependency dependency = new(0, taskId, dependentTaskId);

            s_dalDependency!.Create(dependency);
        }
    }

    public static void Do(ITask? dalTask, IEngineer? dalEngineer, IDependency? dalDependency)
    {
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");

        createEngineers();
        createTasks();
        createDependency();
    }
}
